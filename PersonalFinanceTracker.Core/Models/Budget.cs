using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_Finance_Tracker.Models
{
    public class Budget
    {
        [Key]
        public int Budget_Id { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User Users { get; set; }

        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public Category Categories { get; set; }
    }
}
