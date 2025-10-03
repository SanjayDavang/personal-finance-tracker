using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class BudgetStatusDto
    {
        public int Category_Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal Overspend { get; set; }
    }
}
