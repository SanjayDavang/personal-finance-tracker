namespace PersonalFinanceTracker.UI.Models
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string Category { get; set; }
    }
}
