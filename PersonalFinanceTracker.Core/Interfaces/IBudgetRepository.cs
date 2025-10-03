using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IBudgetRepository
    {
        Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds);
        Task<bool> AddBudgetAsync(Budget budget);
        Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId);
        Task UpdateMonthlyBudgetsAsync(int userId);
        Task<Budget> GetBudgetAsync(int CategoryId);
        Task<List<BudgetStatusDto>> GetAllBudgetStatusAsync(int UserId);
        Task<bool> UpdateBudgetAsync(Budget budget);
    }
}
