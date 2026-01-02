using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task AssignRoleAsync(int userId, string  roleName);
    }
}
