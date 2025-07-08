namespace PersonalFinanceTracker.UI.Models
{
    public class BudgetResponseDto
    {
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
