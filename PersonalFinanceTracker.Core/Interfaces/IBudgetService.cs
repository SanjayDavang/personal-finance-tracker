﻿using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IBudgetService
    {
        Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds);
        Task<bool> AddBudgetAsync(AddBudgetDto addBudgetDto);
        Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId);
        Task CreateNewMonthBudgetsAsync (int userId);
        Task<Budget> GetBudgetAsync(int CategoryId);
        Task<ServiceResponse<bool>> UpdateBudgetAsync(UpdateBudgetDto updateBudgetDto);
    }
}
