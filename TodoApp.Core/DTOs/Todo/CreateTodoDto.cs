using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Attributes;
using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Todo;

public class CreateTodoDto
{
 
    public string Title { get; set; }
    
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    
   
    public Priority Priority { get; set; }
    
    public int CategoryId { get; set; }
  
    public RecurrenceType? RecurrenceType { get; set; }
    
    public int? PlanId { get; set; }
} 