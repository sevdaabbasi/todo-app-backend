using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class NotificationRepository : EfRepository<Notification>
{
    public NotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
} 