namespace TodoApp.Core.Entities;

public class TodoCollaborator : BaseEntity
{
    public int TodoId { get; set; }
    public Todo Todo { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public bool IsAccepted { get; set; }
} 