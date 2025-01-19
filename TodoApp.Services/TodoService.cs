using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<User> _userRepository;

        public TodoService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return (await _userRepository.FindAsync(u => u.Email == email)).FirstOrDefault();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }
    }
}