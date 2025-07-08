namespace PersonalFinanceTracker.UI.Models
{
    public class TransactionResponseDto
    {
        public int Transaction_Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string CategoryName { get; set; }
    }
}
