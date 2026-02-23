using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs.Plan;

public class PlanDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public PlanType PlanType { get; set; }
    public int TodoCount { get; set; }
    public int CollaboratorCount { get; set; }
    public DateTime CreatedAt { get; set; }
} 