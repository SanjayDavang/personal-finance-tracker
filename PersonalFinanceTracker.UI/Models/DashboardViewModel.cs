namespace PersonalFinanceTracker.UI.Models
{
    public class DashboardViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal RemainingBalance { get; set; }
        public List<CategoryExpenseDto> CategoryWiseExpenses { get; set; }
        public List<ExpenseTrendDto> ExpenseTrends { get; set; }
        public List<TransactionDto> LastTransactions { get; set; }
    }

    public class CategoryExpenseDto
    {
        public string Category { get; set; }
        public decimal Total { get; set; }
    }

    public class ExpenseTrendDto
    {
        public string Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
