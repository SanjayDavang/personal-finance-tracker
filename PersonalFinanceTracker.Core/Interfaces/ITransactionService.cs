using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.DTOs;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, string userName);
        Task<List<TransactionResponseDto>> GetAllTransactionsAsync(string userName);
        //Task<bool> DeleteTransactionAsync(int transactionId);
        Task<bool> DeleteTransactionsAsync(List<int> transactionIds);
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
        Task<bool> UpdateTransactionAsync(TransactionResponseDto transactionDto, string userName);
    }
}
