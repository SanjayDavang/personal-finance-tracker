using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceTracker.Core.Models
{
    public class Transaction
    {
        [Key]
        public int Transaction_Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }

        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User Users { get; set; }
        
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public Category Categories { get; set; }

    }
}
