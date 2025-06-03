namespace BackgroundClient.Jobs
{
    using BackgroundClient.Database;
    using CoreBankerApi.Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskProcessingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskProcessingService> _logger;
        private readonly SemaphoreSlim _semaphore;
        private const int MaxRetryAttempts = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(3);

        public TaskProcessingService(IServiceScopeFactory scopeFactory, ILogger<TaskProcessingService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _semaphore = new SemaphoreSlim(3); // Limit to 3 concurrent workers
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Processing Service started.");

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await ProcessPendingTasksAsync(stoppingToken);
            //    await Task.Delay(5000, stoppingToken); // Poll every 5 seconds
            //}

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ProcessPendingTasksAsync(stoppingToken);
                Console.WriteLine($"Task executed at: {DateTime.Now}");
            }

            _logger.LogInformation("Task Processing Service stopped.");
        }

        private async Task ProcessPendingTasksAsync(CancellationToken cancellationToken)
        {
            var processingTasks = new List<Task>();

            //using var _dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();

            //var loanRequest = await _dbContext.LoanRequest.FirstOrDefaultAsync(req => req.Status.ToUpper().Equals("PENDING"));

            //if (loanRequest == null)
            //{

            //    _logger.LogInformation("No Task");
            //    return;
            //}


            while (!cancellationToken.IsCancellationRequested)
            {


                await _semaphore.WaitAsync(cancellationToken);

                //processingTasks.Add(ProcessLoanQueue(loanRequest, cancellationToken));

                //await Task.WhenAll(processingTasks,);

                //_semaphore.Release();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessLoanQueue(cancellationToken);
                        //await ProcessWithRetries(cancellationToken);
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

        private async Task ProcessLoanQueue(CancellationToken cancellationToken)
        {
            using (var _dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {

                    var loanRequest = await _dbContext.LoanRequest
                .FromSqlRaw(@"SELECT TOP 1 * 
                        FROM LoanRequest WITH (UPDLOCK, ROWLOCK, READPAST)
                        WHERE Status = 'Pending'
                        ORDER BY CreatedAt ASC ").FirstOrDefaultAsync();

                    if (loanRequest == null)
                    {

                        return;

                    }


                    var tasks = await _dbContext.TaskQueue
                    .FromSqlRaw(@"
                    SELECT TOP 3 *
                        FROM TaskQueue
                        WHERE Status = 'Pending'
                        AND ReferenceId = {0}
                        ORDER BY CreatedAt ASC", loanRequest.ReferenceId).ToListAsync();

                    if (tasks.Count <= 0)
                        return; // No tasks to process

                    //var task = tasks[0];

                    try
                    {
                        foreach (TaskQueue task in tasks)
                        {
                            int attempt = 0;
                            //while (attempt < MaxRetryAttempts)
                            //{
                                try
                                {
                                    _logger.LogInformation("Processing Task {TaskId}: {TaskType}", task.Id, task.TaskType);

                                    // Simulate processing (replace with real logic) - Core Banking Post
                                    await Task.Delay(10000, cancellationToken);

                                    // Mark task as completed
                                    //var updateTask = task;
                                    //if (updateTask != null)
                                    //{
                                        task.ChangeStatus("Completed");
                                        

                                        //await _dbContext.SaveChangesAsync(cancellationToken);
                                    //}

                                    _logger.LogInformation("Task {TaskId} completed.", task.Id);
                                    attempt = 3; // Success
                                }
                                catch (Exception ex)
                                {
                                    attempt++;
                                    _logger.LogWarning(ex, "Task {TaskId} failed, retrying ({Attempt}/{MaxRetries})...", task.Id, attempt, MaxRetryAttempts);

                                    //if (attempt >= MaxRetryAttempts)
                                    //{
                                        var failedTask = task;
                                        if (failedTask != null)
                                        {
                                            failedTask.ChangeStatus("Failed");
                                            //await _dbContext.SaveChangesAsync(cancellationToken);
                                        }
                                    //    throw;
                                    //}
                                    //await Task.Delay(RetryDelay, cancellationToken);
                                }
                            //}
                        }
                        var status = "Completed";
                        loanRequest.Status = "Completed";


                        await _dbContext.SaveChangesAsync(cancellationToken);

                        //_dbContext.Database.ExecuteSqlInterpolated($"UPDATE LoanRequest SET Status = {status} WHERE ReferenceId = {loanRequest.ReferenceId}");
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }


                }
            }


        }

        private async Task ProcessWithRetries(CancellationToken cancellationToken)
        {
            using var _dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();

            var loanRequest = await _dbContext.LoanRequest
                .FromSqlRaw(@"SELECT TOP 1 * 
                        FROM LoanRequest WITH (UPDLOCK, ROWLOCK, READPAST)
                        WHERE Status = 'Pending'
                        ORDER BY CreatedAt ASC ").FirstOrDefaultAsync();

            if (loanRequest == null)
            {

                return;
               
            }

            //var loanRequest = loanRequests[0];

            _logger.LogInformation($"Request Id {loanRequest.ReferenceId}");


            var tasks = await _dbContext.TaskQueue
                    .FromSqlRaw(@"
                    UPDATE TaskQueue
                    SET Status = 'Processing'
                    OUTPUT INSERTED.*
                    WHERE Id = (
                        SELECT TOP 1 Id 
                        FROM TaskQueue WITH (UPDLOCK, ROWLOCK, READPAST)
                        WHERE Status = 'Pending'
                        AND ReferenceId = {0}
                        ORDER BY CreatedAt ASC
                    )
                ", loanRequest.ReferenceId).ToListAsync();

            if (tasks.Count <= 0)
                return; // No tasks to process

            var task = tasks[0];

            int attempt = 0;
            while (attempt < MaxRetryAttempts)
            {
                try
                {
                    _logger.LogInformation("Processing Task {TaskId}: {TaskType}", task.Id, task.TaskType);

                    // Simulate processing (replace with real logic) - Core Banking Post
                    await Task.Delay(3000, cancellationToken);

                    // Mark task as completed
                    var updateTask = task;
                    if (updateTask != null)
                    {
                        updateTask.ChangeStatus("Completed");
                        loanRequest.Status = "Completed";
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
