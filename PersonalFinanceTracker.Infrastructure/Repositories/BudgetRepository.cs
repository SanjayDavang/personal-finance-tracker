using Microsoft.EntityFrameworkCore;
using Personal_Finance_Tracker.Data;
using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.Infrastructure.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;
        public BudgetRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> AddDefaultBudgetsAsync(int userId, List<int> categoryIds)
        {
            var defaultBudgets = new List<Budget>();
            var today = DateTime.UtcNow;
            DateOnly startDate = new DateOnly(today.Year, today.Month, 1);
            DateOnly endDate = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            foreach (var categoryId in categoryIds)
            {
                defaultBudgets.Add(new Budget
                {
                    Category_Id = categoryId,
                    Amount = 0,
                    StartDate = startDate,
                    EndDate = endDate,
                    User_Id = userId
                });
            }

            _context.Budgets.AddRange(defaultBudgets);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddBudgetAsync(Budget budget)
        {
            _context.Budgets.AddRange(budget);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<BudgetResponseDto>> GetAllBudgetsAsync(int userId)
        {
            return await _context.Budgets
                .Where(b => b.User_Id == userId)
                .Select(b => new BudgetResponseDto 
                {
                    StartDate = b.StartDate, 
                    EndDate = b.EndDate, 
                    Amount = b.Amount, 
                    Category_Id = b.Category_Id
                })
                .ToListAsync();
        }

        public async Task CreateMonthlyBudgetsAsync(int userId)
        {
            var today = DateTime.UtcNow;
            var monthStart = new DateOnly(today.Year, today.Month, 1);
            var monthEnd = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            var existingBudgets = await _context.Budgets
                .Where(b => b.User_Id == userId && b.StartDate == monthStart)
                .ToListAsync();

            if (existingBudgets.Any())
            {
                return; 
            }

            var categories = await _context.Categories
                .Where(c => c.User_Id == userId)
                .ToListAsync();

            var newBudgets = categories.Select(category => new Budget
            {
                User_Id = userId,
                Category_Id = category.Category_Id,
                StartDate = monthStart,
                EndDate = monthEnd,
                Amount = 0  
            }).ToList();

            await _context.Budgets.AddRangeAsync(newBudgets);
            await _context.SaveChangesAsync();
        }

        public async Task<Budget> GetBudgetAsync(int categoryId)
        {
            return await _context.Budgets
                    .Where(b => b.Category_Id == categoryId)
                    .FirstAsync();
        }

        public async Task<bool> UpdateBudgetAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
