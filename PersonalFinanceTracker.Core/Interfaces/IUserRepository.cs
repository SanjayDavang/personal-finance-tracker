using Personal_Finance_Tracker.Models;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
