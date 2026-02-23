using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class CategoryRepository : EfRepository<Category>
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
} 