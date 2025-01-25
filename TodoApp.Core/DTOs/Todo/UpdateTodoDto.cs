using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Todo;

public class UpdateTodoDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [Required]
    public Priority Priority { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public int? PlanId { get; set; }
} 