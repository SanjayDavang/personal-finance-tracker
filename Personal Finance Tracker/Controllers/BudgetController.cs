using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Services;
using PersonalFinanceTracker.Infrastructure.Repositories;

namespace Personal_Finance_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;

        public BudgetController(IUserService userService, IBudgetService budgetService)
        {
            _userService = userService;
            _budgetService = budgetService;
        }


        [HttpPost]
        [Route("AddNewBudget")]
        [Authorize]
        public async Task<IActionResult> AddNewBudget([FromBody] AddBudgetDto addBudgetDto, string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            addBudgetDto.User_Id = user.User_Id;

            var isAdded = await _budgetService.AddBudgetAsync(addBudgetDto);
            return Ok(isAdded);
        }

        [HttpGet]
        [Route("GetAllBudgets")]
        [Authorize]
        public async Task<IActionResult> GetAllBudgets(string userName)
        {
            var user = await _userService.GetUserByUsernameAsync(userName);
            var budgets = await _budgetService.GetAllBudgetsAsync(user.User_Id);
            return Ok(budgets);
        }

        [HttpPost]
        [Route("GenerateMonthlyBudgets")]
        [Authorize]
        public async Task<IActionResult> GenerateMonthlyBudgets(string userName)
        {
            var user = await _userService.GetUserByUsernameAsync(userName);
            if (user == null) return Unauthorized();

            await _budgetService.CreateNewMonthBudgetsAsync(user.User_Id);
            return Ok("Monthly budgets created.");
        }

        [HttpGet]
        [Route("GetBudget")]
        [Authorize]
        public async Task<IActionResult> GetBudget(int categoryId)
        {
            var budget = await _budgetService.GetBudgetAsync(categoryId);
            return Ok(budget);
        }

        [HttpPut]
        [Route("UpdateBudget")]
        [Authorize]
        public async Task<IActionResult> UpdateBudget([FromBody] UpdateBudgetDto updateBudgetDto)
        {
            var result = await _budgetService.UpdateBudgetAsync(updateBudgetDto);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }
    }
}
