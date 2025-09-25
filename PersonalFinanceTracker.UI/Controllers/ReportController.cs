using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.UI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PersonalFinanceTracker.UI.Controllers
{
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7189/api";

        public ReportController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult GetReport() => View();
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetReport(IFormCollection form /*int? year, int? month,DateTime? from, DateTime to*/)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var reportDto = new ReportDto
            {
                ReportType = form["reportType"],
                Month = string.IsNullOrWhiteSpace(form["month"]) ? null : int.Parse(form["month"]),
                Year = string.IsNullOrWhiteSpace(form["year"]) ? null : int.Parse(form["year"]),
                FromDate = string.IsNullOrWhiteSpace(form["fromDate"]) ? null : DateTime.Parse(form["fromDate"]),
                ToDate = string.IsNullOrWhiteSpace(form["toDate"]) ? null : DateTime.Parse(form["toDate"]),
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var apiUrl = $"{_apiBaseUrl}/Report/Export?userName={Uri.EscapeDataString(userName)}";

            var jsonContent = new StringContent(JsonSerializer.Serialize(reportDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
                return View("GetReport"); //Error Baki Aahe

            var pdf = await response.Content.ReadAsByteArrayAsync();
            var contentDisposition = response.Content.Headers.ContentDisposition;
            var fileName = contentDisposition?.FileName?.Trim('"') ?? "Finance_Report.pdf";

            return File(pdf, "application/pdf", fileName);
        }
    }
}
