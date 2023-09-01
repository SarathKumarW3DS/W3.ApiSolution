using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace W3.WebApi.Services.Mail
{
    public interface MailService
    {
        Task SendEmailAsync(string toEmail, string OTP, string subject, string content);
    }
    public class SendGridMailService : MailService
    {
        public IConfiguration _configuration;
        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string OTP, string subject, string content)
        {
            var apiKey = _configuration["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration["FromAddress"],_configuration["MailDisplayName"]);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, OTP);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
