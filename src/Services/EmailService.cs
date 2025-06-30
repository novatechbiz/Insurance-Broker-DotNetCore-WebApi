using InsuraNova.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InsuraNova.Services
{

    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string toEmail, string resetLink);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string resetLink)
        {
            var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Support", _emailConfig.Auth.User));
            message.From.Add(new MailboxAddress("Novalabs", "jsonbrody8@gmail.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Password Reset";

            message.Body = new TextPart("html")
            {
                Text = $"<p>Click the link to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailConfig.Auth.User, _emailConfig.Auth.Pass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
