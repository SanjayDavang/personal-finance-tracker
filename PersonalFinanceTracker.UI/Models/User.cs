using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.UI.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
