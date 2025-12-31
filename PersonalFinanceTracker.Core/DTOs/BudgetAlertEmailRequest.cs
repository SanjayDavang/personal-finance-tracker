using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class BudgetAlertEmailRequest
    {
        public string UserName { get; set; }
        public string Category {  get; set; }
        public decimal Amount { get; set; }
        public decimal OverAmount { get; set; }
    }
}
