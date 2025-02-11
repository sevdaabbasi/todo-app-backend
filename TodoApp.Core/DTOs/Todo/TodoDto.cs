using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Todo;

public class TodoDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority Priority { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public string CategoryName { get; set; }
    public string PlanTitle { get; set; }
    public DateTime CreatedAt { get; set; }
} 