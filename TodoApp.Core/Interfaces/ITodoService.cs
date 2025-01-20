using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface ITodoService
{
    Task<Todo> CreateTodoAsync(Todo todo);
    Task<Todo> GetTodoByIdAsync(int id);
    Task<IEnumerable<Todo>> GetUserTodosAsync(int userId);
    Task<IEnumerable<Todo>> GetTodosByPlanAsync(int planId);
    Task<IEnumerable<Todo>> GetOverdueTodosAsync(int userId);
    Task UpdateTodoAsync(Todo todo);
    Task DeleteTodoAsync(int id);
    Task CompleteTodoAsync(int id);
    Task RescheduleTodoAsync(int todoId, DateTime newDueDate);
}