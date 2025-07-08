namespace PersonalFinanceTracker.UI.Models
{
    public class UpdateBudgateDto
    {
        public int Category_Id { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
