using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface ICategoryService
{
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId);
} 