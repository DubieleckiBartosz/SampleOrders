using System;

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ShopApi.Models;
using ShopApi.Settings;
using ShopApi.Wrappers;

namespace ShopApi.Email
{
    public class EmailService:IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailSettings> emailOptions,ILogger<EmailService> logger)
        {
            _emailSettings = emailOptions.Value;
            _logger = logger;
        }
        public async Task<BaseResponse<bool>> SendEmailAsync(EmailModel model)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
            mailMessage.To.Add(MailboxAddress.Parse(model.To));
            mailMessage.Subject = model.Subject;
            mailMessage.Body = new TextPart(TextFormat.Html) { Text = model.Body };
         
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await smtpClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    await smtpClient.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);
                    await smtpClient.SendAsync(mailMessage);
                    await smtpClient.DisconnectAsync(true);
                }
                return BaseResponse<bool>.Ok(true, "E-mail sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BaseResponse<bool>.Error("Failed to send the email");
            }
        }

    }
}
