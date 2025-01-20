using TodoApp.Core.Interfaces;
using TodoApp.Core.Entities;
using TodoApp.Repositories.Repositories;

namespace TodoApp.Services;

public class CollaborationService : ICollaborationService
{
    private readonly IRepository<TodoCollaborator> _todoCollaboratorRepository;
    private readonly IRepository<PlanCollaborator> _planCollaboratorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CollaborationService(
        IRepository<TodoCollaborator> todoCollaboratorRepository,
        IRepository<PlanCollaborator> planCollaboratorRepository,
        IUnitOfWork unitOfWork)
    {
        _todoCollaboratorRepository = todoCollaboratorRepository;
        _planCollaboratorRepository = planCollaboratorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task InviteUserToTodoAsync(int todoId, int userId)
    {
        var collaborator = new TodoCollaborator
        {
            TodoId = todoId,
            UserId = userId,
            IsAccepted = false
        };
        await _todoCollaboratorRepository.AddAsync(collaborator);
        await _unitOfWork.CommitAsync();
    }

    public async Task InviteUserToPlanAsync(int planId, int userId)
    {
        var collaborator = new PlanCollaborator
        {
            PlanId = planId,
            UserId = userId,
            IsAccepted = false
        };
        await _planCollaboratorRepository.AddAsync(collaborator);
        await _unitOfWork.CommitAsync();
    }

    public async Task AcceptTodoInvitationAsync(int todoId, int userId)
    {
        var collaborator = (await _todoCollaboratorRepository
            .FindAsync(x => x.TodoId == todoId && x.UserId == userId))
            .FirstOrDefault();

        if (collaborator != null)
        {
            collaborator.IsAccepted = true;
            _todoCollaboratorRepository.Update(collaborator);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task AcceptPlanInvitationAsync(int planId, int userId)
    {
        var collaborator = (await _planCollaboratorRepository
            .FindAsync(x => x.PlanId == planId && x.UserId == userId))
            .FirstOrDefault();

        if (collaborator != null)
        {
            collaborator.IsAccepted = true;
            _planCollaboratorRepository.Update(collaborator);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task RemoveUserFromTodoAsync(int todoId, int userId)
    {
        var collaborator = (await _todoCollaboratorRepository
            .FindAsync(x => x.TodoId == todoId && x.UserId == userId))
            .FirstOrDefault();

        if (collaborator != null)
        {
            _todoCollaboratorRepository.Delete(collaborator);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task RemoveUserFromPlanAsync(int planId, int userId)
    {
        var collaborator = (await _planCollaboratorRepository
            .FindAsync(x => x.PlanId == planId && x.UserId == userId))
            .FirstOrDefault();

        if (collaborator != null)
        {
            _planCollaboratorRepository.Delete(collaborator);
            await _unitOfWork.CommitAsync();
        }
    }
} 