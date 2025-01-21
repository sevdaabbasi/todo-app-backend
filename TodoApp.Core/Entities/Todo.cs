using TodoApp.Core.Enums;

namespace TodoApp.Core.Entities;

public class Todo : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority Priority { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int? PlanId { get; set; }
    public Plan Plan { get; set; }
    
    public ICollection<TodoCollaborator> Collaborators { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}