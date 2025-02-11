using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class TodoCollaboratorRepository : EfRepository<TodoCollaborator>
{
    public TodoCollaboratorRepository(AppDbContext context) : base(context)
    {
    }
} 