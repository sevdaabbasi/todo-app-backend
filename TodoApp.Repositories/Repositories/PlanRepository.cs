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
} 