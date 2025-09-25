using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
