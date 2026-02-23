namespace TodoApp.Core.Interfaces;

public interface ICollaborationService
{
    Task InviteUserToTodoAsync(int todoId, int userId);
    Task InviteUserToPlanAsync(int planId, int userId);
    Task AcceptTodoInvitationAsync(int todoId, int userId);
    Task AcceptPlanInvitationAsync(int planId, int userId);
    Task RemoveUserFromTodoAsync(int todoId, int userId);
    Task RemoveUserFromPlanAsync(int planId, int userId);
} 