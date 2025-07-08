using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;

        public CategoryService(ICategoryRepository categoryRepository, IUserService userService, IBudgetService budgetService)
        {
            _categoryRepository = categoryRepository;
            _userService = userService;
            _budgetService = budgetService;
        }

        public async Task<int> GetIdByCategoryAsync(string categoryName, string transactionType, int userId)
        {
            //var categoryId = 0;
            var categoryId = await _categoryRepository.GetIdAsync(categoryName.ToLower(),userId);

            //if (category == null)
            //{
            //    var categoryDto = new CategoryDto{
            //        Name = categoryName.ToLower(),
            //        Type = transactionType,
            //        User_Id = userId
            //    };

            //    var newCategory = await AddNewCategoryAsync(categoryDto);
            //    categoryId = newCategory.Category_Id;
            //}
            //else
            //{
            //    categoryId = category.Category_Id;
            //}
            return categoryId;
        }

        public async Task<int> AddNewCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name.ToLower(),
                Type = categoryDto.Type,
                User_Id = categoryDto.User_Id
            };
            var newCategory = await _categoryRepository.AddAsync(category);

            //var today = DateTime.UtcNow;
            //DateOnly startDate = new DateOnly(today.Year, today.Month, 1);
            //DateOnly endDate = new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            //var addBudgetDto = new AddBudgetDto
            //{
            //    Category_Id = newCategory.Category_Id,
            //    User_Id = categoryDto.User_Id,
            //    Amount = 0,
            //    StartDate = startDate,
            //    EndDate = endDate,
            //};

            //var isAdded = await _budgetService.AddBudgetAsync(addBudgetDto);

            //if (!isAdded) { throw new Exception("Failed to add budget for the new category."); }

            return newCategory.Category_Id;
        }

        public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync(int userId)
        {
            return await _categoryRepository.GetCategoriesAsync(userId);
        }

        public async Task<OperationResultDto> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<ServiceResponse<bool>> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var response = new ServiceResponse<bool>();

            var category = await _categoryRepository.GetCategoryByIdAsync(updateCategoryDto.Category_Id);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found";
                return response;
            }

            category.Name = updateCategoryDto.Name;
            category.Type = updateCategoryDto.Type;

            var updated = await _categoryRepository.UpdateAsync(category);

            if (updated)
            {
                response.Success = true;
                response.Data = true;
                response.Message = "Category Updated Successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to update category.";
            }
            return response;
        }

        public async Task<List<Category>> SearchCategoriesAsync(string query, int userId)
        {
            return await _categoryRepository.SearchAsync(query,userId);
        }

        public async Task<List<int>> AddDefaultCategoriesForUserAsync(int userId)
        {
            return await _categoryRepository.AddDefaultCategoriesAsync(userId);
        }

        public async Task<string> GetNameByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetNameByIdAsync(categoryId);
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetCategoryByIdAsync(categoryId);
        }
    }
}
