namespace TodoApp.Core.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int? TodoId { get; set; }
    public Todo? Todo { get; set; }
} 