using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PersonalFinanceTracker.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7189/api";

        public CategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> SearchCategories(string query)
       {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Category/Search?query={query}&userName={userName}");

            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
                return Json(categories);
            }

            return Json(new { message = "Error retrieving categories" });
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryBudgetResponseDto categoryBudgetResponseDto)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) || userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var categoryDto = new CategoryDto()
            {
                Name = categoryBudgetResponseDto.Name,
                Type = categoryBudgetResponseDto.Type
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiUrl = $"{_apiBaseUrl}/Category/AddCategory?userName={Uri.EscapeDataString(userName)}";

            var jsonContent = new StringContent(JsonSerializer.Serialize(categoryDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var categoryJsonData = await response.Content.ReadAsStringAsync();
                var categoryId = JsonSerializer.Deserialize<int>(categoryJsonData);

                if(categoryId == 0)
                {
                    TempData["ErrorMessage"] = "Failed to Add Category.";
                    return RedirectToAction("GetAllCategories");
                }

                var budgetDto = new AddBudgetDto()
                {
                    Amount = categoryBudgetResponseDto.Amount,
                    StartDate = categoryBudgetResponseDto.StartDate,
                    EndDate = categoryBudgetResponseDto.EndDate,
                    Category_Id = categoryId
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;

                var budgetApiUrl = $"{_apiBaseUrl}/Budget/AddNewBudget?username={userName}";

                var budgetJsonContent = new StringContent(JsonSerializer.Serialize(budgetDto), Encoding.UTF8, "application/json");

                var budgetResponse = await _httpClient.PostAsync(budgetApiUrl, budgetJsonContent);

                if (budgetResponse.IsSuccessStatusCode)
                {
                    var budgetJsondata = await budgetResponse.Content.ReadAsStringAsync();
                    var isAdded = JsonSerializer.Deserialize<bool>(budgetJsondata);

                    if (isAdded)
                    {
                        TempData["SuccessMessage"] = "Category And Its Budget Added Successfully.";
                        return RedirectToAction("GetAllCategories");
                    }

                    TempData["ErrorMessage"] = "Failed to Add Budget.";
                    return RedirectToAction("GetAllCategories");
                }

                TempData["ErrorMessage"] = "Failed to Add Budget.";
                return RedirectToAction("GetAllCategories");
            }

            TempData["ErrorMessage"] = "Category Already Available.";
            return RedirectToAction("GetAllCategories");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString ("username");

            if (string.IsNullOrEmpty(token) && userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Category/GetAllCategoriesWithBudgets?userName={userName}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<CategoryBudgetResponseDto>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var budgetResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Budget/GetAllBudgetStatus?userName={userName}");

                if (budgetResponse.IsSuccessStatusCode)
                {
                    var statusJsonData = await budgetResponse.Content.ReadAsStringAsync();
                    var budgetStatus = JsonSerializer.Deserialize<List<BudgetStatusDto>>(statusJsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ;

                    foreach ( var category in categories)
                    {
                        var budget = budgetStatus.FirstOrDefault(s => s.Category_Id == category.Category_Id);
                        category.Status = budget.Status;
                        category.Overspend = budget.Overspend;
                    }
                }
                return View(categories);
            }
            TempData["ErrorMessage"] = "Failed to load categories.";
            return View(new List<CategoryBudgetResponseDto>());
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) && userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var categoryResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Category/GetCategory?categoryId={id}");

            if (!categoryResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to Edit Category.";
                return RedirectToAction("GetAllCategories");
            }
            var jsonData = await categoryResponse.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<Category>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var budgetResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Budget/GetBudget?categoryId={id}");

            var budgetJsonData = await budgetResponse.Content.ReadAsStringAsync();
            var budget = JsonSerializer.Deserialize<Budget>(budgetJsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (budgetResponse.IsSuccessStatusCode)
            {
                var viewModel = new CategoryBudgetResponseDto()
                {
                    Category_Id = category.Category_Id,
                    Name = category.Name,
                    Type = category.Type,
                    Amount = budget.Amount,
                    StartDate = budget.StartDate,
                    EndDate = budget.EndDate
                };
                return View(viewModel);
            }

            TempData["ErrorMessage"] = "Failed to Edit Category.";
            return RedirectToAction("GetAllCategories");
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryBudgetResponseDto categoryBudgetResponseDto)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) && userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var category = new UpdateCategoryDto
            {
                Category_Id = categoryBudgetResponseDto.Category_Id,
                Name = categoryBudgetResponseDto.Name.ToLower(),
                Type = categoryBudgetResponseDto.Type
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var categoryContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var categoryResponse = await _httpClient.PutAsync($"{_apiBaseUrl}/Category/UpdateCategory", categoryContent);

            if (categoryResponse.IsSuccessStatusCode)
            {
                var budget = new UpdateBudgetDto
                {
                    Category_Id = categoryBudgetResponseDto.Category_Id,
                    Amount = categoryBudgetResponseDto.Amount,
                    StartDate = categoryBudgetResponseDto.StartDate,
                    EndDate = categoryBudgetResponseDto.EndDate
                };

                var budgetContent = new StringContent(JsonSerializer.Serialize(budget), Encoding.UTF8, "application/json");
                var budgetResponse = await _httpClient.PutAsync($"{_apiBaseUrl}/Budget/UpdateBudget", budgetContent);

                if (budgetResponse.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Category and Budget updated successfully";
                    return RedirectToAction("GetAllCategories");
                }

                TempData["ErrorMessage"] = "Category updated, but failed to update Budget.";
                return RedirectToAction("GetAllCategories");
            }

            TempData["ErrorMessage"] = "Failed to Update Category.";
            return RedirectToAction("GetAllCategories");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userName = HttpContext.Session.GetString("username");

            if (string.IsNullOrEmpty(token) && userName == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Category/DeleteCategory?categoryId={id}");

            if (!response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultDto>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("GetAllCategories");
            }

            TempData["SuccessMessage"] = "Category Deleted Successfully.";
            return RedirectToAction("GetAllCategories");
        }

        public IActionResult CategoryInfo()
        {
            return View();
        }
    }
}
