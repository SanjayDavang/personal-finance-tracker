using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;

namespace Personal_Finance_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public DashboardController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("GetDashboardData")]
        [Authorize]
        public async Task<IActionResult> GetDashboardData([FromQuery] string userName, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Get all transactions for the user
            var transactions = await _transactionService.GetAllTransactionsAsync(userName);

            var adjustedEndDate = endDate.Date.AddDays(1).AddTicks(-1);

            // Filter transactions based on date range
            var filteredTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= adjustedEndDate)
                .ToList();

            // Group expenses by category for Pie Chart
            var categoryWiseExpenses = filteredTransactions
                .Where(t => t.TransactionType == "Expense")
                .GroupBy(t => t.CategoryName)
                .Select(g => new CategoryExpenseDto
                {
                    Category = g.Key,
                    Total = g.Sum(t => t.Amount)
                }).ToList();

            // Generate full year (Jan–Dec) for Expense Trends
            var fullYearTrends = Enumerable.Range(1, 12)
                .Select(m => new ExpenseTrendDto
                {
                    Month = new DateTime(startDate.Year, m, 1).ToString("MMM yyyy"),
                    Income = 0,
                    Expense = 0
                })
                .ToList();

            // Group all transactions by month and type
            var monthlyGroups = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    Income = g.Where(t => t.TransactionType == "Income").Sum(t => t.Amount),
                    Expense = g.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount)
                })
                .ToList();

            // Merge actual data into full year
            foreach (var item in monthlyGroups)
            {
                var trend = fullYearTrends.FirstOrDefault(m => m.Month == item.Month);
                if (trend != null)
                {
                    trend.Income = item.Income;
                    trend.Expense = item.Expense;
                }
            }

            var result = new DashboardViewModel
            {
                TotalIncome = filteredTransactions.Where(t => t.TransactionType == "Income").Sum(t => t.Amount),
                TotalExpense = filteredTransactions.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount),
                RemainingBalance = filteredTransactions.Where(t => t.TransactionType == "Income").Sum(t => t.Amount)
                        - filteredTransactions.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount),
                CategoryWiseExpenses = categoryWiseExpenses,
                ExpenseTrends = fullYearTrends,
                LastTransactions = filteredTransactions
                        .OrderByDescending(t => t.Date)
                        .Take(5)
                        .Select(t => new TransactionDto
                        {
                            Amount = t.Amount,
                            Date = t.Date,
                            Description = t.Description,
                            Category = t.CategoryName,
                            TransactionType = t.TransactionType
                        })
                        .ToList()
            }; 

            return Ok(result);
        }
    }
}
