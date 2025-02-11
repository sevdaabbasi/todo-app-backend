using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class UserRepository : EfRepository<User>
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
} 