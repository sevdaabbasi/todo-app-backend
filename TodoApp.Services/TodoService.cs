using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Exceptions;
using TodoApp.Repositories.Repositories;

namespace TodoApp.Services;

public class TodoService : ITodoService
{
    private readonly TodoRepository _todoRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public TodoService(
        TodoRepository todoRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Todo> CreateTodoAsync(Todo todo)
    {
        try
        {
            await ValidateTodoAsync(todo);
            
            await _todoRepository.AddAsync(todo);
            await _unitOfWork.CommitAsync();

            if (todo.DueDate <= DateTime.UtcNow.AddDays(1))
            {
                await _notificationService.SendDueDateReminderAsync(todo.Id);
            }

            return todo;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new BusinessException("Failed to create todo.", ex);
        }
    }

    public async Task<Todo> GetTodoByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new NotFoundException(nameof(Todo), id);
        }
        return todo;
    }

    public async Task<IEnumerable<Todo>> GetUserTodosAsync(int userId)
    {
        return await _todoRepository.FindAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Todo>> GetTodosByPlanAsync(int planId)
    {
        return await _todoRepository.GetTodosByPlanAsync(planId);
    }

    public async Task<IEnumerable<Todo>> GetOverdueTodosAsync(int userId)
    {
        try
        {
            var overdueTodos = await _todoRepository.GetOverdueTodosAsync(userId);
            
            foreach (var todo in overdueTodos)
            {
                await _notificationService.SendOverdueNotificationAsync(todo.Id);
            }
            
            return overdueTodos;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to get overdue todos.", ex);
        }
    }

    public async Task UpdateTodoAsync(Todo todo)
    {
        try
        {
            await ValidateTodoAsync(todo);
            
            var existingTodo = await GetTodoByIdAsync(todo.Id);
            
            if (existingTodo.DueDate != todo.DueDate && 
                todo.DueDate <= DateTime.UtcNow.AddDays(1))
            {
                await _notificationService.SendDueDateReminderAsync(todo.Id);
            }

            _todoRepository.Update(todo);
            await _unitOfWork.CommitAsync();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new BusinessException("Failed to update todo.", ex);
        }
    }

    public async Task DeleteTodoAsync(int id)
    {
        try
        {
            var todo = await GetTodoByIdAsync(id);
            _todoRepository.Delete(todo);
            await _unitOfWork.CommitAsync();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new BusinessException("Failed to delete todo.", ex);
        }
    }

    public async Task CompleteTodoAsync(int id)
    {
        try
        {
            var todo = await GetTodoByIdAsync(id);
            
            if (todo.IsCompleted)
            {
                throw new BusinessException("Todo is already completed.");
            }

            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.UtcNow;
            
            _todoRepository.Update(todo);
            await _unitOfWork.CommitAsync();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new BusinessException("Failed to complete todo.", ex);
        }
    }

    public async Task RescheduleTodoAsync(int todoId, DateTime newDueDate)
    {
        try
        {
            if (newDueDate < DateTime.UtcNow)
            {
                throw new ValidationException("Due date cannot be in the past.");
            }

            var todo = await GetTodoByIdAsync(todoId);
            
            todo.DueDate = newDueDate;
            _todoRepository.Update(todo);
            await _unitOfWork.CommitAsync();
            
            if (newDueDate <= DateTime.UtcNow.AddDays(1))
            {
                await _notificationService.SendDueDateReminderAsync(todoId);
            }
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new BusinessException("Failed to reschedule todo.", ex);
        }
    }

    private async Task ValidateTodoAsync(Todo todo)
    {
        if (todo == null)
        {
            throw new ValidationException("Todo cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(todo.Title))
        {
            throw new ValidationException("Title is required.");
        }

        if (todo.DueDate < DateTime.UtcNow)
        {
            throw new ValidationException("Due date cannot be in the past.");
        }

        // Kategori kontrolü
        if (todo.CategoryId > 0)
        {
            var categoryExists = await _todoRepository
                .AnyAsync(x => x.CategoryId == todo.CategoryId);
                
            if (!categoryExists)
            {
                throw new ValidationException("Invalid category.");
            }
        }

        // Plan kontrolü
        if (todo.PlanId.HasValue)
        {
            var planExists = await _todoRepository
                .AnyAsync(x => x.PlanId == todo.PlanId);
                
            if (!planExists)
            {
                throw new ValidationException("Invalid plan.");
            }
        }
    }
}