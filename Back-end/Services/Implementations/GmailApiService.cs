using EventosPro.Services.Interfaces;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using MimeKit;
using System.Text;


public class GmailApiService : IGmailService, IDisposable
{
    private readonly GmailService _gmailService;
    private readonly ILogger<GmailApiService> _logger;
    private readonly string _senderEmail;
    private bool _disposed;

    public GmailApiService(GmailAuthenticator authenticator, GmailService gmailService, ILogger<GmailApiService> logger, IConfiguration configuration)
    {
        _gmailService = authenticator.AuthenticateAsync().GetAwaiter().GetResult();
        _logger = logger;
        _senderEmail = configuration["GmailSettings:SenderEmail"] ?? throw new ArgumentNullException(nameof(_senderEmail));
        _disposed = false;
    }

    public async Task<string> SendEmailAsync(string to, string subject, string htmlBody)  
    {
        try
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_senderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html") { Text = htmlBody }; 

            var rawMessage = Base64UrlEncode(mimeMessage.ToString());
            var message = new Message { Raw = rawMessage };

            var request = _gmailService.Users.Messages.Send(message, "me");
            var response = await request.ExecuteAsync();

            _logger.LogInformation("Email sent successfully to {recipient}", to);
            return response.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {recipient}", to);
            throw new Exception("Failed to send email", ex);
        }
    }

    private static string Base64UrlEncode(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _gmailService?.Dispose();
            }
            _disposed = true;
        }
    }
}