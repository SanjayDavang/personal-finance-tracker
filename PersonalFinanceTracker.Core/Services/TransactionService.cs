using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;


namespace PersonalFinanceTracker.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ICategoryService _categoryService;
        private readonly IBudgetService _budgetService;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ICategoryService categoryService, IUserRepository userRepository, ITransactionRepository transactionRepository, IBudgetService budgetService)
        {
            _categoryService = categoryService;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _budgetService = budgetService;
        }
        public async Task<Transaction> AddTransactionAsync(TransactionDto transactionDto, string userName)
        {

            var user = await _userRepository.GetByUsernameAsync(userName);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var userId = user.User_Id;
            var categoryId = await _categoryService.GetIdByCategoryAsync(transactionDto.Category,transactionDto.TransactionType,userId);

            if(categoryId == 0)
            {
                var category = new CategoryDto()
                {
                    Name = transactionDto.Category,
                    Type = transactionDto.TransactionType,
                    User_Id = userId,
                };
                categoryId = await _categoryService.AddNewCategoryAsync(category);

                if (categoryId == 0)
                {
                    throw new Exception("Failed to add new Category");
                }

                var today = DateTime.UtcNow;
                DateOnly startDate = new DateOnly(today.Year, today.Month, 1);
                DateOnly endDate = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

                var budget = new AddBudgetDto()
                {
                    Amount = 0,
                    StartDate = startDate,
                    EndDate = endDate,
                    Category_Id = categoryId,
                    User_Id = userId
                };

                var isAdded = await _budgetService.AddBudgetAsync(budget);

                if (!isAdded)
                {
                    throw new Exception("Failed to add Budget for new Category");
                }
            }

            var transaction = new Transaction
            {
                Date = transactionDto.Date,
                Amount = transactionDto.Amount,
                Description = transactionDto.Description,
                TransactionType = transactionDto.TransactionType,
                User_Id = userId,
                Category_Id = categoryId
            };

            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task<List<TransactionResponseDto>> GetAllTransactionsAsync(string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var transactions = await _transactionRepository.GetTransactionsAsync(user.User_Id);

            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactionRepository.GetByIdAsync(transactionId);
        }

        public async Task<bool> DeleteTransactionAsync(int TransactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(TransactionId);
            if (transaction == null)
            {
                return false;
            }

            await _transactionRepository.DeleteAsync(transaction);
            return true;
        }

        public async Task<bool> UpdateTransactionAsync(TransactionResponseDto transactionDto, string userName)
        {
            var existing = await _transactionRepository.GetByIdAsync(transactionDto.Transaction_Id);

            if (existing == null) { return false; }

            var user = await _userRepository.GetByUsernameAsync(userName);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var categoryId = await _categoryService.GetIdByCategoryAsync(transactionDto.CategoryName, transactionDto.TransactionType, user.User_Id);

            existing.Date = transactionDto.Date;
            existing.Amount = transactionDto.Amount;
            existing.Description = transactionDto.Description;
            existing.TransactionType = transactionDto.TransactionType;
            existing.Category_Id = categoryId;
            existing.User_Id = user.User_Id;

            return await _transactionRepository.UpdateTransactionAsync(existing);
        }
    }
}
