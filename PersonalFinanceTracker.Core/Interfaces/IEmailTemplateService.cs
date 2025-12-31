using PersonalFinanceTracker.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> GetBudgetAlertBodyAsync(BudgetAlertEmailRequest request);
    }
}
