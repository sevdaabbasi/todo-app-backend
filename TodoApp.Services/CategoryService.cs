using TodoApp.Core.Entities;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces;


namespace TodoApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private ICategoryService _categoryServiceImplementation;

        public CategoryService(IRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return category;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with id {id} was not found");
            return category;
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId)
        {
            return await _categoryRepository.FindAsync(c => c.UserId == userId);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                _categoryRepository.Update(category);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to update category", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetByIdAsync(id);
            _categoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId)
        {
            return _categoryServiceImplementation.GetTodosByCategoryAsync(categoryId);
        }
    }
}