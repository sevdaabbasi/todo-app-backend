namespace TodoApp.Core.Entities;

public class PlanCollaborator : BaseEntity
{
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public bool IsAccepted { get; set; }
} 