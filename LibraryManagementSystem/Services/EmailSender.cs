using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<AuthOptions> options, ILogger<EmailSender> logger)
        {
            _apiKey = options.Value.ApiKey; 
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress("Add your Email ");
                var toEmail = new EmailAddress(to);
                var plainTextContent = message;
                var htmlContent = message;
                var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                _logger.LogInformation("Email sent with status code: {statusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email: {errorMessage}", ex.Message);
                throw; 
            }
        }
    }
}
