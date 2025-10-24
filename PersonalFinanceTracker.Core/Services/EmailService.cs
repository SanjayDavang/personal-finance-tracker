using MailKit.Security;
using MimeKit;
using PersonalFinanceTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Personal Finance Tracker", "personalfinance025@gmail.com"));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("personalfinance025@gmail.com", "xcgk xome izpt dgbi");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
