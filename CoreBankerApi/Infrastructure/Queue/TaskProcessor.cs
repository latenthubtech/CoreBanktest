namespace CoreBankerApi.Infrastructure.Queue
{
    using CoreBankerApi.Domain.Models;
    using CoreBankerApi.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskProcessingService> _logger;
        private readonly SemaphoreSlim _semaphore;
        private const int MaxRetryAttempts = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(2);

        public TaskProcessor(IServiceScopeFactory scopeFactory, ILogger<TaskProcessingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _semaphore = new SemaphoreSlim(3); // Limit to 3 concurrent workers
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Processing Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessPendingTasksAsync(stoppingToken);
                await Task.Delay(1000, stoppingToken); // Poll every 5 seconds
            }

            _logger.LogInformation("Task Processing Service stopped.");
        }

        private async Task ProcessPendingTasksAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskQueueContext>();

            while (!cancellationToken.IsCancellationRequested)
            {
                

                await _semaphore.WaitAsync(cancellationToken);
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessWithRetries(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Task {TaskId} failed after retries.");
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }, cancellationToken);
            }
        }

        private async Task ProcessWithRetries(CancellationToken cancellationToken)
        {
            using var _dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueContext>();

            var tasks = await _dbContext.TaskQueue
                    .FromSqlRaw(@"
                    UPDATE TaskQueue
                    SET Status = 'Processing'
                    OUTPUT INSERTED.*
                    WHERE Id = (
                        SELECT TOP 1 Id 
                        FROM TaskQueue WITH (UPDLOCK, ROWLOCK, READPAST)
                        WHERE Status = 'Pending'
                        ORDER BY CreatedAt ASC
                    )
                ").ToListAsync();

            if (tasks.Count <= 0)
                return; // No tasks to process

            var task = tasks[0];

            int attempt = 0;
            while (attempt < MaxRetryAttempts)
            {
                try
                {
                    _logger.LogInformation("Processing Task {TaskId}: {TaskType}", task.Id, task.TaskType);

                    // Simulate processing (replace with real logic)
                    await Task.Delay(3000, cancellationToken);

                    // Mark task as completed
                    var updateTask = task;
                    if (updateTask != null)
                    {
                        updateTask.ChangeStatus("Completed");
                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }

                    _logger.LogInformation("Task {TaskId} completed.", task.Id);
                    break; // Success
                }
                catch (Exception ex)
                {
                    attempt++;
                    _logger.LogWarning(ex, "Task {TaskId} failed, retrying ({Attempt}/{MaxRetries})...", task.Id, attempt, MaxRetryAttempts);

                    if (attempt >= MaxRetryAttempts)
                    {
                        var failedTask = task;
                        if (failedTask != null)
                        {
                            failedTask.ChangeStatus("Failed");
                            await _dbContext.SaveChangesAsync(cancellationToken);
                        }
                        throw;
                    }
                    await Task.Delay(RetryDelay, cancellationToken);
                }
            }
        }
    }

}
