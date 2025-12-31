using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Models;

namespace PersonalFinanceTracker.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Personal Finance Tracker", _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }


    }
}
