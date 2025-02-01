using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using Microsoft.Extensions.Logging;
using TodoApp.Core.DTOs.Todo;
using TodoApp.Core.Exceptions;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoService todoService, ILogger<TodoController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    private async Task ValidateUserAccessToTodoAsync(int todoId, int userId)
    {
        var todo = await _todoService.GetTodoByIdAsync(todoId);
        if (todo.UserId != userId && !todo.Collaborators.Any(c => c.UserId == userId))
        {
            throw new UnauthorizedAccessException("You don't have access to this todo.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoDto dto)
    {
        try 
        {
            var userId = int.Parse(User.Identity?.Name ?? "0");
            
            var todo = new Todo
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                RecurrenceType = dto.RecurrenceType,
                CategoryId = dto.CategoryId,
                PlanId = dto.PlanId,
                UserId = userId
            };
            
            var createdTodo = await _todoService.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.Id }, createdTodo);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return UnprocessableEntity(new { 
                error = new { 
                    message = ex.Message,
                    type = ex.GetType().Name,
                    details = ex.InnerException?.Message 
                }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoById(int id)
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        await ValidateUserAccessToTodoAsync(id, userId);
        
        var todo = await _todoService.GetTodoByIdAsync(id);
        return Ok(todo);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserTodos()
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        var todos = await _todoService.GetUserTodosAsync(userId);
        return Ok(todos);
    }

    [HttpGet("plan/{planId}")]
    public async Task<IActionResult> GetTodosByPlan(int planId)
    {
        var todos = await _todoService.GetTodosByPlanAsync(planId);
        return Ok(todos);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdueTodos()
    {
        var userId = int.Parse(User.Identity?.Name ?? "0");
        var todos = await _todoService.GetOverdueTodosAsync(userId);
        return Ok(todos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, [FromBody] UpdateTodoDto dto)
    {
        try 
        {
            var userId = int.Parse(User.Identity?.Name ?? "0");
            
            var existingTodo = await _todoService.GetTodoByIdAsync(id);
            if (existingTodo == null)
                return NotFound();
            
            if (existingTodo.UserId != userId)
                return Unauthorized();

            // Mevcut todo'yu güncelle
            existingTodo.Title = dto.Title;
            existingTodo.Description = dto.Description;
            existingTodo.DueDate = dto.DueDate;
            existingTodo.Priority = dto.Priority;
            existingTodo.RecurrenceType = dto.RecurrenceType;
            existingTodo.CategoryId = dto.CategoryId;
            existingTodo.PlanId = dto.PlanId;

            await _todoService.UpdateTodoAsync(existingTodo);
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return UnprocessableEntity(new { 
                error = new { 
                    message = ex.Message,
                    type = ex.GetType().Name,
                    details = ex.InnerException?.Message 
                }
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        await _todoService.DeleteTodoAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteTodo(int id)
    {
        await _todoService.CompleteTodoAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reschedule")]
    public async Task<IActionResult> RescheduleTodo(int id, [FromBody] DateTime newDueDate)
    {
        await _todoService.RescheduleTodoAsync(id, newDueDate);
        return NoContent();
    }
} 