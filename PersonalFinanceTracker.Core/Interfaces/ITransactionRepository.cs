using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.DTOs;

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
