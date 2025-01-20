using TodoApp.Core.Enums;

namespace TodoApp.Core.Entities;

public class Plan : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public PlanType PlanType { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<Todo> Todos { get; set; }
    public ICollection<PlanCollaborator> Collaborators { get; set; }
} 