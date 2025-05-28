using Microsoft.Extensions.Configuration;
using Application.Utilities.Contractors;
using System.Net;
using System.Net.Mail;

namespace Application.Services.Email
{
    public sealed class EmailSender(IConfiguration _configuration) : INotificationSender
    {
        public async Task SendAsync(string To, string Title, string Body, Dictionary<string, string>? Data = null)
        {
            SmtpClient smtpClient = new()
            {
                Port = int.Parse(_configuration["MailSettings:Port"]),
                Host = _configuration["MailSettings:Host"],
                EnableSsl = false,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"])
            };

            string senderEmail = _configuration["MailSettings:Mail"];
            string senderDisplayName = _configuration["MailSettings:DisplayName"];

            MailAddress senderAddress = new(senderEmail, senderDisplayName);
            MailAddress toAddress = new(To);
            MailMessage mailMessage = new(senderAddress, toAddress)
            {
                Subject = Title,
                Body = Body,
                IsBodyHtml = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}