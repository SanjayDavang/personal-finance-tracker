using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        public ReportService(ITransactionRepository transactionRepository, IUserRepository userRepository ) 
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }


        public async Task<byte[]> GenerateReportPdfAsync(ReportDto reportDto, string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
                var userId = user.User_Id;

                DateTime startDate, endDate;

            switch (reportDto.ReportType.ToLower())
            {
                case "month":
                    startDate = new DateTime(reportDto.Year ?? DateTime.Now.Year, reportDto.Month ?? 1, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case "year":
                    startDate = new DateTime(reportDto.Year ?? DateTime.Now.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddDays(-1);
                    break;
                case "custom":
                    startDate = reportDto.FromDate ?? DateTime.Now;
                    endDate = reportDto.ToDate ?? DateTime.Now;
                    break;
                default:
                    throw new ArgumentException("Invalid report type");
            }

            var transactions = await _transactionRepository.GetTransactionsByDateRange(startDate, endDate, userId);
            var income = transactions.Where(t => t.TransactionType == "Income").Sum(t => t.Amount);
            var expense = transactions.Where(t => t.TransactionType == "Expense").Sum(t => t.Amount);
            var balance = income - expense;

            return PdfHelper.CreatePdf(income, expense, balance, transactions);
        }
    }
}
