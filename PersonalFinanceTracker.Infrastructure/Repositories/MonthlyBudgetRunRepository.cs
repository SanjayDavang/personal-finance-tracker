using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Infrastructure.Repositories
{
    public class MonthlyBudgetRunRepository : IMonthlyBudgetRunRepository
    {
        private readonly AppDbContext _context;

        public MonthlyBudgetRunRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastRunDateAsync(int userId)
        {
            var record = await _context.MonthlyBudgetRuns
                .FirstOrDefaultAsync(x => x.UserId == userId);
            return record?.LastRunDate;
        }

        public async Task SetLastRunDateAsync(int userId, DateTime runDate)
        {
            var record = await _context.MonthlyBudgetRuns
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (record == null)
            {
                _context.MonthlyBudgetRuns.Add(new MonthlyBudgetRun
                {
                    UserId = userId,
                    LastRunDate = runDate
                });
            }
            else
            {
                record.LastRunDate = runDate;
            }
            await _context.SaveChangesAsync();
        }
    }
}
