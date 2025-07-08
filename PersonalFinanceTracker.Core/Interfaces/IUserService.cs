﻿using Personal_Finance_Tracker.Models;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
