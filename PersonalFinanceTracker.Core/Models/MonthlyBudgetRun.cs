using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Models
{
    public class MonthlyBudgetRun
    {
        public int Id { get; set; }
        public int  UserId { get; set; }
        public DateTime LastRunDate { get; set; }
    }
}
