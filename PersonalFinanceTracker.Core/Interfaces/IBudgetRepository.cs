using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.DTOs;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IBudgetRepository
    {
        Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds);
        Task<bool> AddBudgetAsync(Budget budget);
        Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId);
        Task UpdateMonthlyBudgetsAsync(int userId, DateTime today);
        Task<Budget> GetBudgetAsync(int CategoryId);
        Task<List<BudgetStatusDto>> GetAllBudgetStatusAsync(int UserId);
        Task<bool> UpdateBudgetAsync(Budget budget);
        Task<BudgetStatusResponse> GetBudgetStatusAsync(int UserId, int CategoryId, decimal Amount);
    }
}
