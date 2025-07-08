using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> GetIdAsync(string category, int userId);
        Task<string> GetNameByIdAsync(int categoryId);
        Task<Category> AddAsync(Category category);
        Task<List<CategoryResponseDto>> GetCategoriesAsync(int userId);
        Task<OperationResultDto> DeleteAsync(int id);
        Task<bool> UpdateAsync(Category category);
        Task<List<Category>> SearchAsync(string query, int userId);
        Task<List<int>> AddDefaultCategoriesAsync(int userId);
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
