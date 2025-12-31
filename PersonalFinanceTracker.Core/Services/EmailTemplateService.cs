using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> GetBudgetAlertBodyAsync(BudgetAlertEmailRequest request)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var templatePath = Path.Combine(basePath, "EmailTemplates", "BudgetAlert.html");

            var html = await File.ReadAllTextAsync(templatePath);

            return html.Replace("{{UserName}}", request.UserName)
                       .Replace("{{Category}}", request.Category)
                       .Replace("{{OverAmount}}", request.OverAmount.ToString("N0"))
                       .Replace("{{Amount}}", request.Amount.ToString("N0"));
        }
    }
}
