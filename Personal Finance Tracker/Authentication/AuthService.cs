using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ICategoryService _categoryService;
        private readonly IBudgetService _budgetService;

        public AuthService(IUserService userService, IConfiguration config, ICategoryService categoryService, IBudgetService budgetService)
        {
            _userService = userService;
            _config = config;
            _categoryService = categoryService;
            _budgetService = budgetService;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach(var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RegisterUserAsync(string email, string username, string password)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(username);
            var existingEmail = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null) return false;
            if (existingEmail != null) return false;

            var user = new User
            {
                UserName = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = email
            };

            var newUser = await _userService.AddUserAsync(user);

            await _userService.AssignRoleAsync(newUser.User_Id, "User");

            var categoryIds = await _categoryService.AddDefaultCategoriesForUserAsync(newUser.User_Id);
            if(categoryIds == null) return false;

            return await _budgetService.AddDefaultBudgetsAsync(newUser.User_Id, categoryIds);
        }
    }
}
