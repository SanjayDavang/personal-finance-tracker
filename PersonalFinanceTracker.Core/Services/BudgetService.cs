using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        public BudgetService(IBudgetRepository budgetRepository) 
        {
            _budgetRepository = budgetRepository;
        }
        public async Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds)
        {
            return await _budgetRepository.AddDefaultBudgetsAsync(userId, categoryIds);
        }

        public async Task<bool> AddBudgetAsync(AddBudgetDto addBudgetDto)
        {
            var budget = new Budget();

            budget.Category_Id = addBudgetDto.Category_Id; ;
            budget.Amount = addBudgetDto.Amount;
            budget.StartDate = addBudgetDto.StartDate;
            budget.EndDate = addBudgetDto.EndDate;
            budget.User_Id = addBudgetDto.User_Id;

            return await _budgetRepository.AddBudgetAsync(budget);
        }

        public async Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId)
        {
            return await _budgetRepository.GetAllBudgetsAsync(userId);
        }

        public async Task UpdateBudgetsForNewMonthAsync(int userId)
        {
            await _budgetRepository.UpdateMonthlyBudgetsAsync(userId);
        }

        public async Task<Budget> GetBudgetAsync(int categoryId)
        {
            return await _budgetRepository.GetBudgetAsync(categoryId);
        }

        public async Task<List<BudgetStatusDto>> GetAllBudgetStatusAsync(int UserId)
        {
            return await _budgetRepository.GetAllBudgetStatusAsync(UserId);
        }

        public async Task<ServiceResponse<bool>> UpdateBudgetAsync(UpdateBudgetDto updateBudget)
        {
            var response = new ServiceResponse<bool>();

            var budget = await _budgetRepository.GetBudgetAsync(updateBudget.Category_Id);

            if (budget == null)
            {
                response.Success = false;
                response.Message = "Budget Not Found.";
                return response;
            }

            budget.Amount = updateBudget.Amount;
            budget.StartDate = updateBudget.StartDate;
            budget.EndDate = updateBudget.EndDate;

            var updated = await _budgetRepository.UpdateBudgetAsync(budget);

            if (updated)
            {
                response.Success = true;
                response.Data = true;
                response.Message = "Budget Updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to update budget.";
            }

            return response;
        }
    }
}
