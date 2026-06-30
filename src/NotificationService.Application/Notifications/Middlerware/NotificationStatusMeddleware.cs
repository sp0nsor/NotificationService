using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Application.Notifications.Context;
using NotificationService.Core.Models;
using NotificationService.Core.Primitives.Enums;

namespace NotificationService.Application.Notifications.Middlerware
{
    public sealed class NotificationStatusMeddleware
    {
        public async Task FinallyAsync(
            NotificationExecutionContext context,
            IBaseRepository<Notification> repository,
            Exception? exception,
            CancellationToken cancellationToken)
        {
            if (exception is not null)
            {
                if (context.Notification is not null)
                {
                    context.Notification.SetLastError(exception.Message);
                    context.Notification.ChangeStatus(Status.Failed);

                    await repository.UpdateAsync(
                        context.Notification,
                        cancellationToken);
                }

                Console.WriteLine($"Error {DateTime.UtcNow}: {exception.Message}");
            }
            else
            {
                if (context.Notification is not null)
                {
                    await repository.UpdateAsync(
                        context.Notification,
                        cancellationToken);
                }
            }
        }
    }
}
