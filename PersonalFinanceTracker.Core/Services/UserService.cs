using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }
    }
}
