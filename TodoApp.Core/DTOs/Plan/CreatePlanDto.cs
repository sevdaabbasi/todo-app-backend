using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Plan;

public class CreatePlanDto
{
    [Required]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public PlanType PlanType { get; set; }
} 