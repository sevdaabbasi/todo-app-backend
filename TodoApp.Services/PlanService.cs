using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Repositories.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Services;

public class PlanService : IPlanService
{
    private readonly PlanRepository _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlanService(PlanRepository planRepository, IUnitOfWork unitOfWork)
    {
        _planRepository = planRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Plan> CreatePlanAsync(Plan plan)
    {
        await _planRepository.AddAsync(plan);
        await _unitOfWork.CommitAsync();
        return plan;
    }

    public async Task<Plan> GetPlanByIdAsync(int id)
    {
        return await _planRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Plan>> GetUserPlansAsync(int userId)
    {
        return await _planRepository.FindAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Plan>> GetActivePlansAsync(int userId)
    {
        return await _planRepository.GetActivePlansAsync(userId);
    }

    public async Task UpdatePlanAsync(Plan plan)
    {
        _planRepository.Update(plan);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeletePlanAsync(int id)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        if (plan != null)
        {
            _planRepository.Delete(plan);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task AddCollaboratorAsync(int planId, int userId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan != null)
        {
            var collaborator = new PlanCollaborator
            {
                PlanId = planId,
                UserId = userId,
                IsAccepted = false
            };
            plan.Collaborators.Add(collaborator);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task RemoveCollaboratorAsync(int planId, int userId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan != null)
        {
            var collaborator = plan.Collaborators.FirstOrDefault(x => x.UserId == userId);
            if (collaborator != null)
            {
                plan.Collaborators.Remove(collaborator);
                await _unitOfWork.CommitAsync();
            }
        }
    }

    private async Task ValidatePlanAsync(Plan plan)
    {
        if (plan == null)
        {
            throw new ValidationException("Plan cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(plan.Title))
        {
            throw new ValidationException("Title is required.");
        }

        if (plan.StartDate >= plan.EndDate)
        {
            throw new ValidationException("End date must be after start date.");
        }

        if (plan.StartDate < DateTime.UtcNow)
        {
            throw new ValidationException("Start date cannot be in the past.");
        }
    }
} 