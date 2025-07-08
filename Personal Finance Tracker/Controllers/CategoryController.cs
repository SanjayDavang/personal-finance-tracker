using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Infrastructure.Repositories;

namespace Personal_Finance_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserRepository _userRepository;
        private readonly IBudgetService _budgetService;

        public CategoryController(ICategoryService categoryService, IUserRepository userRepository, IBudgetService budgetService)
        {
            _categoryService = categoryService;
            _userRepository = userRepository;
            _budgetService = budgetService;
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> SearchCategories(string query, string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            var categories = await _categoryService.SearchCategoriesAsync(query.ToLower(),user.User_Id);
            return Ok(categories);
        }

        [HttpPost]
        [Route("AddCategory")]
        [Authorize]
        public async Task<IActionResult> AddNewCategory([FromBody] CategoryDto categoryDto, string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            categoryDto.Name = categoryDto.Name.ToLower();
            categoryDto.User_Id = user.User_Id;

            var oldCategoryId  = await _categoryService.GetIdByCategoryAsync(categoryDto.Name,categoryDto.Type,categoryDto.User_Id);

            if(oldCategoryId == 0)
            {
                var categoryId = await _categoryService.AddNewCategoryAsync(categoryDto);
                return Ok(categoryId);
            }

            return BadRequest(new { success = false, message = "Category Already available" });
        }

        [HttpGet]
        [Route("GetAllCategoriesWithBudgets")]
        [Authorize]
        public async Task<IActionResult> GetAllCategoriesWithBudgets(string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            var categories = await _categoryService.GetAllCategoriesAsync(user.User_Id);
            var budgets = await _budgetService.GetAllBudgetsAsync(user.User_Id);

            var result = (from category in categories
                          join budget in budgets on category.Category_Id equals budget.Category_Id
                          select new CategoryBudgetResponseDto
                          {
                              Category_Id = category.Category_Id,
                              Name = category.Name,
                              Type = category.Type,
                              Amount = budget.Amount,
                              StartDate = budget.StartDate,
                              EndDate = budget.EndDate
                          }).ToList();

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            if (!result.Success)
            {
                return BadRequest(new {success = result.Success, message = result.Message });
            }

            return Ok(new { success = result.Success, message = result.Message });
        }

        [HttpPut]
        [Route("UpdateCategory")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var result = await _categoryService.UpdateCategoryAsync(updateCategoryDto);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        [HttpGet]
        [Route("GetCategoryName")]
        [Authorize]
        public async Task<IActionResult> GetCategoryName(int categoryId)
        {
            var categoryName = await _categoryService.GetNameByIdAsync(categoryId);
            return Ok(categoryName);    
        }

        [HttpGet]
        [Route("GetCategory")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            return Ok(category);
        }
    }
}
