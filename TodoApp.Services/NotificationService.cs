using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Repositories.Repositories;

namespace TodoApp.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(NotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateNotificationAsync(Notification notification)
    {
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
    {
        return await _notificationRepository.FindAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
    {
        return await _notificationRepository.GetUnreadNotificationsAsync(userId);
    }

    public async Task MarkNotificationAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            _notificationRepository.Update(notification);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task DeleteNotificationAsync(int id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification != null)
        {
            _notificationRepository.Delete(notification);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task SendDueDateReminderAsync(int todoId)
    {
        var notification = new Notification
        {
            Title = "Görev Hatırlatması",
            Message = "Bir görevin yaklaşan son tarihi var!",
            TodoId = todoId,
            IsRead = false
        };
        await CreateNotificationAsync(notification);
    }

    public async Task SendOverdueNotificationAsync(int todoId)
    {
        var notification = new Notification
        {
            Title = "Gecikmiş Görev",
            Message = "Bir görevin süresi doldu!",
            TodoId = todoId,
            IsRead = false
        };
        await CreateNotificationAsync(notification);
    }

    public async Task SendReschedulingRequestAsync(int todoId)
    {
        var notification = new Notification
        {
            Title = "Yeniden Planlama Talebi",
            Message = "Bir görev için yeniden planlama gerekiyor.",
            TodoId = todoId,
            IsRead = false
        };
        await CreateNotificationAsync(notification);
    }
} 