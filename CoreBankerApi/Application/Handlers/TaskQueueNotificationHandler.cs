namespace CoreBankerApi.Application.Handlers
{
    using CoreBankerApi.Application.Notification;
    using CoreBankerApi.Infrastructure.Queue;
    using MediatR;
    using Microsoft.AspNetCore.SignalR.Protocol;
    using Microsoft.IdentityModel.Tokens;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskQueueNotificationHandler : INotificationHandler<TaskQueueNotification>
    {
        private readonly TaskQueueService _queueService;
        private string taskMessage;

        public TaskQueueNotificationHandler(TaskQueueService queueService) { 
            _queueService = queueService;
        }
        public async Task Handle(TaskQueueNotification notification, CancellationToken cancellationToken)
        {
            await _queueService.EnqueueTaskAsync(taskMessage, 4);
        }
    }

}
