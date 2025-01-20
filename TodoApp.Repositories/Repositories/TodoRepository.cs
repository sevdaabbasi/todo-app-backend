using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class TodoRepository : EfRepository<Todo>
{
    public TodoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Todo>> GetOverdueTodosAsync(int userId)
    {
        return await _context.Todos
            .Where(x => x.UserId == userId && 
                       !x.IsCompleted && 
                       x.DueDate < DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetTodosByPlanAsync(int planId)
    {
        return await _context.Todos
            .Where(x => x.PlanId == planId)
            .ToListAsync();
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await _context.Categories.AnyAsync(x => x.Id == categoryId);
    }

    public async Task<bool> PlanExistsAsync(int planId)
    {
        return await _context.Plans.AnyAsync(x => x.Id == planId);
    }
} 