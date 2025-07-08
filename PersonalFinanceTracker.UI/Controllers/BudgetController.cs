using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PersonalFinanceTracker.UI.Controllers
{
    public class BudgetController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7189/api";

        public IActionResult GetAllBudgets()
        {
            return View();
        }

        public async Task<IActionResult> GenerateBudgets()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) && userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Budget/GenerateMonthlyBudgets?userName={userName}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Budgets generated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to generate budgets.";
            }

            return RedirectToAction("BudgetList");
        }
    }
}
