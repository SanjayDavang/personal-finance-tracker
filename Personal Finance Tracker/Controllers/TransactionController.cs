using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal_Finance_Tracker.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Services;

namespace Personal_Finance_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Add")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto, [FromQuery] string userName)
        {
            if (transactionDto == null)
            {
                return BadRequest(new { success = false, message = "Transaction data is required." });
            }

            var transaction = await _transactionService.AddTransactionAsync(transactionDto, userName);

            if (transaction == null)
            {
                return BadRequest(new { success = false, message = "Transaction could not be added. Please try again." });
            }

            return Ok(new { success = true, message = "Transaction added successfully." });
        }

        [HttpGet]
        [Route("GetAllTransactions")]
        [Authorize]
        public async Task<IActionResult> GetAllTransactions(string username)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync(username);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteTransaction")]
        [Authorize]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            try
            {
                var response = await _transactionService.DeleteTransactionAsync(transactionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetTransactionById")]
        [Authorize]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            try
            {
                var response = await _transactionService.GetTransactionByIdAsync(transactionId);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});  
            }
        }

        [HttpPut]
        [Route("UpdateTransaction")]
        [Authorize]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionResponseDto transactionResponseDto, [FromQuery] string userName)
        {
            try
            {
                var response = await _transactionService.UpdateTransactionAsync(transactionResponseDto, userName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
