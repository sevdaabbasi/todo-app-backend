using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface IPlanService
{
    Task<Plan> CreatePlanAsync(Plan plan);
    Task<Plan> GetPlanByIdAsync(int id);
    Task<IEnumerable<Plan>> GetUserPlansAsync(int userId);
    Task<IEnumerable<Plan>> GetActivePlansAsync(int userId);
    Task UpdatePlanAsync(Plan plan);
    Task DeletePlanAsync(int id);
    Task AddCollaboratorAsync(int planId, int userId);
    Task RemoveCollaboratorAsync(int planId, int userId);
} 