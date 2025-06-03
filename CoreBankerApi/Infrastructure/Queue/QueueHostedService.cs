using CoreBankerApi.Domain.Models;
using CoreBankerApi.Infrastructure.Queue;
using System.Threading;

namespace CoreBankerApi.Infrastructure.Queue
{
    using CoreBankerApi.Domain.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueuedHostedService : BackgroundService
    {
        private readonly TaskQueueService _taskQueueService;
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly SemaphoreSlim _semaphore; // Limits concurrent processing
        private readonly IServiceScopeFactory _serviceScopeFactory;
        

        public QueuedHostedService(ILogger<QueuedHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _semaphore = new SemaphoreSlim(5); // Allow up to 5 tasks to be processed concurrently
            _serviceScopeFactory = serviceScopeFactory;
            
           _taskQueueService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueService>(); 

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Queue Processor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = await _taskQueueService.DequeueBatchAsync(5); // Fetch 5 tasks at a time

                if (tasks.Count > 0)
                {
                    var processingTasks = new List<Task>();

                    foreach (var task in tasks)
                    {
                        processingTasks.Add(ProcessTaskAsync(task));
                    }

                    await Task.WhenAll(processingTasks);
                }
                //else
                //{
                    await Task.Delay(5000, stoppingToken); // Reduce CPU usage when no tasks are available
                //}
            }
        }

        private async Task ProcessTaskAsync(TaskQueue task)
        {
            


            try
            {
                _logger.LogInformation($"Processing Task {task.Id}");

                // Simulate processing
                //await Task.Delay(2000);

                _logger.LogInformation($"Task {task.Id} completed successfully.");
                await _taskQueueService.MarkTaskCompletedAsync(task.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Task {task.Id} failed, retrying...");
                await _taskQueueService.RetryTaskAsync(task.Id);
            }
            finally
            {
                _semaphore.Release();
            }
        }
            }


}
