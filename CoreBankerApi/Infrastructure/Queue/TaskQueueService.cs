namespace CoreBankerApi.Infrastructure.Queue
{
    using CoreBankerApi.Domain.Models;
    using CoreBankerApi.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TaskQueueService
    {
        //private TaskQueueContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private TaskQueueContext _dbContext;

        public TaskQueueService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        // Enqueue a new task
        public async Task EnqueueTaskAsync(string taskData, int priority)
        {
            using var _dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueContext>();
            var referenceId = "";
            TaskQueue task = null;
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    referenceId = Guid.NewGuid().ToString();
                    var loanRequest = new LoanRequest
                    {
                        ReferenceId = referenceId,
                        Id = ++i
                    };
                
                    await _dbContext.LoanRequest.AddAsync(loanRequest);

                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 1", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task);
                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 2", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task);
                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 3", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task); 
                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 4", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task);
                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 5", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task);
                    task = TaskQueue.NewInstance(referenceId, "DISBURSE - 6", "Data", loanRequest.Id);
                    await _dbContext.TaskQueue.AddAsync(task);

                }
                
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
            }
            finally
            {
                //_dbContext.Dispose();
            }

            
        }

        // Fetch tasks in batch
        public async Task<List<TaskQueue>> DequeueBatchAsync(int batchSize)
        {
            var lockToken = Guid.NewGuid();

            using var _dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueContext>();

            var tasks = await _dbContext.TaskQueue
                .Where(t => t.Status == "Pending")
                .Take(batchSize)
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.ChangeStatus("Processing");
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return tasks;
            }
            catch (DbUpdateConcurrencyException)
            {
                return new List<TaskQueue>(); // Handle concurrency safely
            }
        }

        // Mark task as completed
        public async Task MarkTaskCompletedAsync(int taskId)
        {
            using var _dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueContext>();
            using var transaction = _dbContext.Database.BeginTransaction();

            await _dbContext.TaskQueue
                .Where(t => t.Id == taskId)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(x => x.Status, "Completed"));

            await transaction.CommitAsync();

            //_dbContext.Dispose();
        }

        // Retry failed tasks with retry limit
        public async Task RetryTaskAsync(int taskId)
        {
            using var _dbContext = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TaskQueueContext>();

            var task = await _dbContext.TaskQueue.FindAsync(taskId);
            if (task != null)
            {
                //task.RetryCount++;

                //if (task.RetryCount > 3)
                //    task.Status = "Failed";
                //else
                //    task.Status = "Pending"; // Retry task

                //task.LockToken = null;
                //task.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
            }
        }
    }


}
