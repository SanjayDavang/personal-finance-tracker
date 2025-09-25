using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<List<TransactionResponseDto>> GetTransactionsAsync(int userId);
        //Task DeleteAsync(Transaction transaction);
        Task<bool> DeleteTransactionsAsync(List<int> transactionIds);
        Task<Transaction> GetByIdAsync(int transactionId);
        Task<bool> UpdateTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsByDateRange(DateTime startDate, DateTime endDate, int userId);
    }
}
