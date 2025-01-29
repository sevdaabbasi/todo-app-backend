using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Attributes;
using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Todo;

public class CreateTodoDto
{
   // [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
   // [Required(ErrorMessage = "Due date is required")]
    //[DataType(DataType.DateTime)]
   // [FutureDate(ErrorMessage = "Due date must be in the future")]
    public DateTime DueDate { get; set; }
    
   // [Required(ErrorMessage = "Priority is required")]
   // [Range(0, 2, ErrorMessage = "Priority must be between 0 and 2")]
    public Priority Priority { get; set; }
    
    public int CategoryId { get; set; }
   // [Required(ErrorMessage = "Category is required")]
    public RecurrenceType? RecurrenceType { get; set; }
    
    public int? PlanId { get; set; }
} 