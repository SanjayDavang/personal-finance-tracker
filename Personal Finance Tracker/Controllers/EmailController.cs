using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.DTOs;
using PersonalFinanceTracker.Core.Interfaces;

namespace Personal_Finance_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailController(IEmailService emailService, IUserService userService, IEmailTemplateService emailTemplateService)
        {
            _emailService = emailService;
            _userService = userService;
            _emailTemplateService = emailTemplateService;
        }

        [HttpPost]
        [Route("SendBudgetAlert")]
        public async Task SendBudgetAlert(BudgetAlertEmailRequest request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.UserName);
            var htmlBody = await _emailTemplateService.GetBudgetAlertBodyAsync(request);
            try
            {
                await _emailService.SendEmailAsync(user.Email, "Budget Exceeded Alert", htmlBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email send error: {ex.Message}");
                throw;
            }
            
        }
    }
}
