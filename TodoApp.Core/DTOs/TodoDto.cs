using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs;

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
    public int CategoryId { get; set; }
    public int? PlanId { get; set; }
} 