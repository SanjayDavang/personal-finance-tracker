using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IBudgetService
    {
        Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds);
        Task<bool> AddBudgetAsync(AddBudgetDto addBudgetDto);
        Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId);
        Task UpdateBudgetsForNewMonthAsync(int userId, DateTime month);
        Task<Budget> GetBudgetAsync(int CategoryId);
        Task<List<BudgetStatusDto>> GetAllBudgetStatusAsync(int UserId);
        Task<ServiceResponse<bool>> UpdateBudgetAsync(UpdateBudgetDto updateBudgetDto);
        Task<BudgetStatusResponse> GetBudgetStatusAsync(BudgetStatusRequest request);

    }
}
