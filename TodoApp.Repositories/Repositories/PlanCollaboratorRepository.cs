using TodoApp.Core.Entities;

namespace TodoApp.Repositories.Repositories;

public class PlanCollaboratorRepository : EfRepository<PlanCollaborator>
{
    public PlanCollaboratorRepository(AppDbContext context) : base(context)
    {
    }
} 