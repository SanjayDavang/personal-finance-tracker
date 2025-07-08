namespace PersonalFinanceTracker.UI.Models
{
    public class AddBudgetDto
    {
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int User_Id { get; set; }
        public int Category_Id { get; set; }
    }
}
