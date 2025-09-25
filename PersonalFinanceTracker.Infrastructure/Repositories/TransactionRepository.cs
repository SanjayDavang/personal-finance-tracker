using Microsoft.EntityFrameworkCore;
using Personal_Finance_Tracker.Data;
using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<List<TransactionResponseDto>> GetTransactionsAsync(int userId)
        {
            return await _context.Transactions
            .Where(t => t.User_Id == userId) 
            .Join(_context.Categories, 
                  t => t.Category_Id,
                  c => c.Category_Id,
                  (t, c) => new TransactionResponseDto
                  {
                      Transaction_Id = t.Transaction_Id,
                      Date = t.Date,
                      Amount = t.Amount,
                      Description = t.Description,
                      TransactionType = t.TransactionType,
                      CategoryName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Name), 
                  })
            .ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(int trasactionId)
        {
            return await _context.Transactions.FindAsync(trasactionId);
        }

        //public async Task DeleteAsync(Transaction transaction)
        //{
        //    _context.Transactions.Remove(transaction);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<bool> DeleteTransactionsAsync(List<int> transactionIds)
        {
            var transactions = await _context.Transactions
                .Where(t => transactionIds.Contains(t.Transaction_Id))
                .ToListAsync();

            if (transactions.Any())
            {
                _context.Transactions.RemoveRange(transactions);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> GetTransactionsByDateRange(DateTime startDate, DateTime endDate, int userId)
        {
            return await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate && t.User_Id == userId)
                .ToListAsync();
        }
    }
}
