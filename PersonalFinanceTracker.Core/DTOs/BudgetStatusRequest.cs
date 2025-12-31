using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class BudgetStatusRequest
    {
        public string UserName {  get; set; }
        public string Category {  get; set; }
        public string TransactionType {  get; set; }
        public decimal Amount {  get; set; }

    }
}
