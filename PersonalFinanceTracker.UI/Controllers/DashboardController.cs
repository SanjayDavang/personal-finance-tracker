using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinanceTracker.UI.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PersonalFinanceTracker.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7189/api";

        public DashboardController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string filter = "ThisMonth", DateTime? startDate = null, DateTime? endDate = null)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var today = DateTime.UtcNow;

            // Handle Filter Logic
            switch (filter)
            {
                case "Last7Days":
                    startDate = today.AddDays(-7);
                    endDate = today;
                    break;

                case "LastMonth":
                    startDate = new DateTime(today.Year, today.Month - 1, 1);
                    endDate = startDate.Value.AddMonths(1).AddDays(-1);
                    break;

                case "ThisMonth":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = today;
                    break;

                case "Custom":
                    if (!startDate.HasValue || !endDate.HasValue)
                    {
                        return BadRequest("Custom date range requires both startDate and endDate.");
                    }
                    break;

                default:
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = today;
                    break;
            }

            var startDateStr = startDate?.ToString("yyyy-MM-dd") ?? "";
            var endDateStr = endDate?.ToString("yyyy-MM-dd") ?? "";

            // Fetch Dashboard Data
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Dashboard/GetDashboardData?userName={userName}&startDate={startDateStr}&endDate={endDateStr}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var jsonData = await response.Content.ReadAsStringAsync();
            var dashboardData = JsonConvert.DeserializeObject<DashboardViewModel>(jsonData);

            return View(dashboardData);
        }
    }
}
