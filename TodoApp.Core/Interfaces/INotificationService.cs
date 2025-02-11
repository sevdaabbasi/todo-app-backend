using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
    Task MarkNotificationAsReadAsync(int notificationId);
    Task DeleteNotificationAsync(int id);
    Task SendDueDateReminderAsync(int todoId);
    Task SendOverdueNotificationAsync(int todoId);
    Task SendReschedulingRequestAsync(int todoId);
} 