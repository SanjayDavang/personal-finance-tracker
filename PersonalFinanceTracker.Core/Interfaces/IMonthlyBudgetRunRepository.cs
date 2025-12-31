using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IMonthlyBudgetRunRepository
    {
        Task<DateTime?> GetLastRunDateAsync(int userId);
        Task SetLastRunDateAsync(int userId, DateTime runDate);
    }
}
