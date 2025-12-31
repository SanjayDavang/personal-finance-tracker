using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class BudgetStatusResponse
    {
        public int Category_Id { get; set; }
        public bool IsOverBudget { get; set; }
        public decimal OverAmount { get; set; }
    }
}
