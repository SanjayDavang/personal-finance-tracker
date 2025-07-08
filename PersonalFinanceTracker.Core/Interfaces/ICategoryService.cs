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
    public interface ICategoryService
    {
        Task<int> GetIdByCategoryAsync(string category, string transactionType, int userId);
        Task<string> GetNameByIdAsync(int categoryId);
        Task<int> AddNewCategoryAsync(CategoryDto categoryDto);
        Task<List<CategoryResponseDto>> GetAllCategoriesAsync(int usreId);
        Task<OperationResultDto> DeleteCategoryAsync(int id);
        Task<ServiceResponse<bool>> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task<List<Category>> SearchCategoriesAsync(string query, int UserId);
        Task<List<int>> AddDefaultCategoriesForUserAsync(int userId);
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
