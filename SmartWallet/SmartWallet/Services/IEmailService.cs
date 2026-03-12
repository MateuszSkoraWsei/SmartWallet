using Azure.Communication.Email;
using Azure;


namespace SmartWallet.Services

{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
    public class AzureEmailService : IEmailService
    {
        private readonly string _connectionString;
        private readonly string _senderAddress;
        public AzureEmailService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureCommunicationService:ConnectionString"];
            _senderAddress = configuration["AzureCommunicationService:SenderAddress"];
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            if (string.IsNullOrEmpty(_connectionString) || string.IsNullOrEmpty(_senderAddress))
            {
                return false;
            }
            
            try
            {
                var emailClient = new EmailClient(_connectionString);
                var emailMessage = new EmailMessage(
                    senderAddress: _senderAddress,
                    recipientAddress: toEmail,
                    content: new EmailContent(subject)
                    {
                        Html = htmlContent
                    }
                );
                var response = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);

                return response.Value.Status == EmailSendStatus.Succeeded;
            }
            catch (RequestFailedException ex)
            {
                
                return false;
            }
        }
    }
}

