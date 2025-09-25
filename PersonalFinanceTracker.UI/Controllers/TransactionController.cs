using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.UI.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PersonalFinanceTracker.UI.Controllers
{
    public class TransactionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7189/api";

        public TransactionController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult AddTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDto transactionDto)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiUrl = $"{_apiBaseUrl}/Transaction/Add?userName={Uri.EscapeDataString(userName)}";

            var jsonContent = new StringContent(JsonSerializer.Serialize(transactionDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ModelState.Clear();
                TempData["SuccessMessage"] = "Transaction added successfully.";
                return View();
            }

            TempData["ErrorMessage"] = "Failed to add transaction.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions() 
        {
            var token = HttpContext.Session.GetString("JWToken");

            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiUrl = $"{_apiBaseUrl}/Transaction/GetAllTransactions?userName={Uri.EscapeDataString(userName)}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var transactions = JsonSerializer.Deserialize<List<TransactionResponseDto>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(transactions);
            }

            TempData["ErrorMessage"] = "Failed to load transactions.";
            return View(new List<TransactionResponseDto>());
        }

        [HttpGet]
        public async Task<IActionResult> EditTransaction(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Transaction/GetTransactionById?transactionId={id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Transaction not found.";
                return RedirectToAction("GetAllTransactions");
            }
            var jsonData = await response.Content.ReadAsStringAsync();
            var transaction = JsonSerializer.Deserialize<Transaction> (jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var categoryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Category/GetCategoryName?categoryId={transaction.Category_Id}");
            var categoryName = "Unknown";

            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryContent = await categoryResponse.Content.ReadAsStringAsync();
                categoryName = categoryContent.Trim('"');
            }

            var viewModel = new TransactionResponseDto
            {
                Transaction_Id = transaction.Transaction_Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                TransactionType = transaction.TransactionType,
                CategoryName = categoryName
            };
            return View(viewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(TransactionResponseDto transactionResponseDto)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var apiUrl = $"{_apiBaseUrl}/Transaction/UpdateTransaction?userName={Uri.EscapeDataString(userName)}";
            var jsonContent = new StringContent(JsonSerializer.Serialize(transactionResponseDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ModelState.Clear();
                TempData["SuccessMessage"] = "Transaction updated successfully.";
                return View();
            }

            TempData["ErrorMessage"] = "Failed to update transaction.";
            return RedirectToAction("GetAllTransactions");
        }

        //[HttpGet]
        //public async Task<IActionResult> DeleteTransaction(int id)
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("AccessDenied", "Account");
        //    }
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Transaction/DeleteTransaction?transactionId={id}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        TempData["SuccessMessage"] = "Deleted transaction successfully.";
        //        return RedirectToAction("GetAllTransactions");
        //    }
        //    TempData["ErrorMessage"] = "Failed to Delete transaction.";
        //    return RedirectToAction("GetAllTransactions");
        //}

        [HttpGet]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var idList = ids.Split(',').Select(int.Parse).ToList();
            var queryString = string.Join("&", idList.Select(id => $"TransactionIds={id}"));

            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Transaction/DeleteTransactions?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Selected transactions deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete selected transactions.";
            }

            return RedirectToAction("GetAllTransactions");
        }
    }
}
