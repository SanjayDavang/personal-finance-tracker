using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Infrastructure.Data;
using PersonalFinanceTracker.Core.Models;
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

        public async Task<BudgetStatusResponse>GetBudgetStatusAsync(int UserId, int CategoryId, decimal Amount)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.User_Id == UserId &&
                                                                    b.Category_Id == CategoryId);
            if (budget == null || budget.Amount == 0)
            {
                return new BudgetStatusResponse
                {
                    Category_Id = CategoryId,
                    OverAmount = 0,
                    IsOverBudget = false
                };
            }

            var totalSpend = await _context.Transactions
                .Where(t =>  t.User_Id == UserId 
                && t.Category_Id == CategoryId 
                && DateOnly.FromDateTime(t.Date) >= budget.StartDate
                && DateOnly.FromDateTime(t.Date) <= budget.EndDate)
                .SumAsync(t => t.Amount);


            if(totalSpend > budget.Amount)
            {
                var overSpend = totalSpend - budget.Amount;

                return new BudgetStatusResponse
                {
                    Category_Id = CategoryId,
                    OverAmount = overSpend,
                    IsOverBudget = true
                };
            }

            return new BudgetStatusResponse
            {
                Category_Id = CategoryId,
                OverAmount = 0, 
                IsOverBudget = false                
            };

        }

        public async Task UpdateMonthlyBudgetsAsync(int userId, DateTime today)
        {
            var monthStart = new DateOnly(today.Year, today.Month, 1);
            var monthEnd = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            var existingBudgets = await _context.Budgets
                .Where(b => b.User_Id == userId)
                .ToListAsync();

            if (!existingBudgets.Any())
            {
                return; 
            }

            foreach (var budget in existingBudgets)
            {
                budget.StartDate = monthStart;
                budget.EndDate = monthEnd;
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<Budget> GetBudgetAsync(int categoryId)
        {
            return await _context.Budgets
                    .Where(b => b.Category_Id == categoryId)
                    .FirstAsync();
        }

        public async Task<List<BudgetStatusDto>> GetAllBudgetStatusAsync(int UserId)
        {
            var result = await (from c in _context.Categories
                          join b in _context.Budgets on c.Category_Id equals b.Category_Id
                          let totalExpenses = _context.Transactions
                                                      .Where(t => t.Category_Id == c.Category_Id 
                                                      && t.TransactionType == "Expense"
                                                      && DateOnly.FromDateTime(t.Date) >= b.StartDate
                                                      && DateOnly.FromDateTime(t.Date) <= b.EndDate)
                                                      .Sum(t => (decimal?)t.Amount) ?? 0
                          select new BudgetStatusDto
                          {
                              Category_Id = c.Category_Id,
                              Name = c.Name,
                              Status = totalExpenses > b.Amount ? "Over Budget" : "Within Budget",
                              Overspend = totalExpenses - b.Amount
                          })
              .ToListAsync();

            return result;
        }

        public async Task<bool> UpdateBudgetAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
