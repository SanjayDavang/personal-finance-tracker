using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Models;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Infrastructure.Repositories;

namespace Personal_Finance_Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.AuthenticateAsync(loginDto.UserName, loginDto.Password);
            if (token == null) return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            bool isRegistered = await _authService.RegisterUserAsync(registerDto.Email, registerDto.UserName, registerDto.Password);
            if (!isRegistered) return BadRequest("Username already exists");
            return Ok("You're all set! Please login in to continue.");
        }
    }
}
