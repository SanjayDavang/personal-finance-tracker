using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Infrastructure.Data;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.Services;
using System.Globalization;

namespace PersonalFinanceTracker.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context; 

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetIdAsync(string categoryName, int userId)
        {
            return await _context.Categories
                .Where(c => c.Name == categoryName &&  c.User_Id == userId)
                .Select(c => c.Category_Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<CategoryResponseDto>> GetCategoriesAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.User_Id == userId)
                .Select(c => new CategoryResponseDto
                {
                    Category_Id = c.Category_Id,
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Name),
                    Type = c.Type,
                })
                .ToListAsync();
        }

        public async Task<OperationResultDto> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Category_Id == id);

            if (category == null)
            {
                return new OperationResultDto(false,"Category Not Found!");
            }

            if (category.Transactions.Any())
            {
                return new OperationResultDto(false,"Cannot delete category. It is associated with transactions.");
            }

           _context.Categories.Remove(category);
           await _context.SaveChangesAsync();
            return new OperationResultDto(true,"Category deleted successfully."); 
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();

            return result > 0;            
        }

        public async Task<List<Category>> SearchAsync(string query, int userId)
        {
            return await _context.Categories
            .Where(c => c.Name.Contains(query) && c.User_Id == userId)  
            .Select(c => new Category { Category_Id = c.Category_Id, Name = c.Name })
            .ToListAsync();
        }

        public async Task<List<int>> AddDefaultCategoriesAsync(int userId)
        {
            var defaultCategories = DefaultCategoryService.GetDefaultCategories(userId);
            await _context.Categories.AddRangeAsync(defaultCategories);
            _context.SaveChanges();
            return defaultCategories.Select(c => c.Category_Id).ToList();
        }

        public async Task<string> GetNameByIdAsync(int categoryId)
        {
            var category = await _context.Categories
                .Where(c => c.Category_Id == categoryId)
                .Select(c => c.Name)
                .FirstAsync();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(category);
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories
                .Where(c => c.Category_Id == categoryId)
                .FirstAsync();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            category.Name = textInfo.ToTitleCase(category.Name);

            return category;
        }
    }
}
