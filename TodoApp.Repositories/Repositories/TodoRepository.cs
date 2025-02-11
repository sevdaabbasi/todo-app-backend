using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class TodoRepository : EfRepository<Todo>
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context) : base(context)
    {
        _context = context;
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

    public async Task<bool> CategoryExistsAsync(int categoryId, int userId)
    {
        return await _context.Categories
            .AnyAsync(x => x.Id == categoryId && x.UserId == userId);
    }

    public async Task<bool> PlanExistsAsync(int planId, int userId)
    {
        return await _context.Plans
            .AnyAsync(x => x.Id == planId && x.UserId == userId);
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        return await _context.Todos
            .Include(t => t.Category)
            .Include(t => t.Plan)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
} 