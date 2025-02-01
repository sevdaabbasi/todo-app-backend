using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class PlanRepository : EfRepository<Plan>
{
    public PlanRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Plan>> GetActivePlansAsync(int userId)
    {
        return await _context.Plans
            .Where(x => x.UserId == userId && 
                       x.EndDate >= DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<Plan> GetByIdAsync(int id)
    {
        return await _context.Plans
            .Include(p => p.Collaborators)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
} 