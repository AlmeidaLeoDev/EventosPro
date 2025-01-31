using EventosPro.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace EventosPro.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient; 
        private readonly ICryptographyService _cryptographyService;
        private readonly ILogger<EmailService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly string _email;

        public EmailService(SmtpClient smtpClient, ICryptographyService cryptographyService, ILogger<EmailService> logger, IJwtTokenService jwtTokenService)
        {
            _smtpClient = smtpClient;
            _cryptographyService = cryptographyService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        public EmailService()
        {
            string email = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string encryptedPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(encryptedPassword))
            {
                throw new ApplicationException("As variáveis de ambiente EMAIL_ADDRESS ou EMAIL_PASSWORD não estão configuradas corretamente.");
            }

            string decryptedPassword = _cryptographyService.Decrypt(encryptedPassword);

            _email = email;

            _smtpClient = new SmtpClient("smtp.gmail.com") 
            {
                Port = 587, 
                Credentials = new NetworkCredential(email, decryptedPassword), 
                EnableSsl = true,
            };
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationToken, string name, string confirmationLink)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
            ArgumentException.ThrowIfNullOrEmpty(confirmationToken, nameof(confirmationToken));
            ArgumentException.ThrowIfNullOrEmpty(confirmationLink, nameof(confirmationLink));

            try
            {
                var expirationTime = _jwtTokenService.GetExpirationTime(); 
                var expirationDays = Math.Ceiling((expirationTime - DateTime.UtcNow).TotalDays);

                confirmationLink = $"https://yourdomain.com/confirm-email?token={confirmationToken}";

                string emailBody = $@"
                <p>Olá, {name}</p>
                <p>Obrigado por se registrar no EventosPro!</p>
                <p>Por favor, confirme seu endereço de e-mail clicando no link abaixo:</p>
                <p><a href='{confirmationLink}'>Confirmar E-mail</a></p>
                <p>Este link expirará em {expirationDays} dia.</p>
                <p>Se você não solicitou este registro, ignore este e-mail.</p>";

                await SendEmailAsync(
                    email,
                    "Confirmação de E-mail - EventosPro",
                    emailBody
                );

                _logger.LogInformation("Confirmation email sent to {Name}, {Email}", name, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email to {Email}", email);
                throw new ApplicationException("Failed to send confirmation email", ex);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            ArgumentException.ThrowIfNullOrEmpty(to, nameof(to));
            ArgumentException.ThrowIfNullOrEmpty(subject, nameof(subject));
            ArgumentException.ThrowIfNullOrEmpty(body, nameof(body));

            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(_email),
                    Subject = subject, 
                    Body = body, 
                    IsBodyHtml = true 
                };
                mailMessage.To.Add(to); 

                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {EmailAddress}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {EmailAddress}", to);
                throw new ApplicationException("Failed to send email", ex);
            }
        }

    }
}
