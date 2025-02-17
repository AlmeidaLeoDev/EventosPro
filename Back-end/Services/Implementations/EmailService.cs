using EventosPro.Services.Interfaces;


namespace EventosPro.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IGmailService _gmailService;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly string _baseUrl;

        public EmailService(
            IGmailService gmailService,
            ILogger<EmailService> logger,
            IJwtTokenService jwtTokenService,
            IConfiguration configuration)
        {
            _gmailService = gmailService;
            _logger = logger;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _baseUrl = _configuration.GetValue<string>("AppSettings:BaseUrl");
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationToken, string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
            ArgumentException.ThrowIfNullOrEmpty(confirmationToken, nameof(confirmationToken));
            ArgumentException.ThrowIfNullOrEmpty(name);

            try
            {
                _logger.LogInformation("Preparando email de confirmação para {Name} ({Email})", name, email);

                var expirationTime = _jwtTokenService.GetExpirationTime();
                var expirationDays = (_jwtTokenService.GetExpirationTime() - DateTime.UtcNow).Days;

                var confirmationLink = BuildConfirmationLink(_baseUrl, confirmationToken);
                _logger.LogInformation("Link de confirmação gerado: {Link}", confirmationLink);

                var emailBody = await BuildEmailConfirmationTemplate(name, confirmationLink, expirationDays);
                await _gmailService.SendEmailAsync(email, "Confirmação de email", emailBody);

                _logger.LogInformation("Confirmation email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no envio de confirmação");
                throw;
            }
        }

        public async Task SendEventInviteAsync(string email, string userName, string eventDescription, DateTime startTime)
        {
            try
            {
                _logger.LogInformation("Preparing event invitation for {UserName} ({Email})", userName, email);

                var emailBody = await BuildEventInviteTemplate(userName, eventDescription, startTime);
                await _gmailService.SendEmailAsync(email, "Convite para evento - EventosPro", emailBody);

                _logger.LogInformation("Event invitation sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no envio do convite");
                throw;
            }
        }

        private string BuildConfirmationLink(string _baseUrl, string token)
        {
            return $"{_baseUrl.TrimEnd('/')}/confirm-email?token={token}";
        }

        private Task<string> BuildEmailConfirmationTemplate(string name, string confirmationLink, int expirationDays)
        {
            // In a real app, this would load from a template file
            var template = $@"
        <html>
            <head>
                <meta charset='UTF-8'>
            </head>
            <body>
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h1>Olá, {name}</h1>
                    <p>Obrigado por se registrar no EventosPro!</p>
                    <p>Por favor, confirme seu endereço de e - mail clicando no link abaixo:</p>
                    <p><a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 14px 20px; text-decoration: none; border-radius: 4px;'>Confirmar Email</a></p>
                    <p>Este link expirará em {expirationDays} dias.</p>
                    <p>Se você não solicitou este registro, ignore este email.</p>
                </div>
            </body>
        </html>";


            return Task.FromResult(template);
        }

        private Task<string> BuildEventInviteTemplate(string userName, string eventDescription, DateTime startTime)
        {
            var template = $@"

        <html>
            <head>
                <meta charset='UTF-8'>
            </head>
            <body>
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2>Olá {userName},</h2>
                    <p>Você foi convidado para o seguinte evento:</p>
                    <div style='background-color: #f8f9fa; padding: 20px; border-radius: 4px;'>
                        <h3>{eventDescription}</h3>
                        <p>Data e hora: {startTime:dd/MM/yyyy HH:mm}</p>
                    </div>
                    <p>Por favor, faça login na sua conta para responder a este convite.</p>
                </div>;
            </body>
        </html>";

            return Task.FromResult(template);
        }
    }

}