using EventosPro.Services.Interfaces;
using MimeKit;
using MailKit.Security;
using EventosPro.Models;

namespace EventosPro.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ILogger<EmailService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly EmailSettings _emailSettings;
        private readonly string _emailPassword;

        public EmailService(ICryptographyService cryptographyService, ILogger<EmailService> logger, IJwtTokenService jwtTokenService, EmailSettings emailSettings)
        {
            _cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _emailSettings = emailSettings;

            _logger.LogInformation("Inicializando EmailService...");

            string encryptedPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            if (string.IsNullOrEmpty(_emailSettings.FromEmail) || string.IsNullOrEmpty(encryptedPassword))
            {
                _logger.LogError("Variáveis de ambiente ausentes ou vazias: EMAIL_ADDRESS={EmailAddress}, EMAIL_PASSWORD={HasPassword}",
                    _emailSettings.FromEmail,
                    !string.IsNullOrEmpty(encryptedPassword));
                throw new ApplicationException("As variáveis de ambiente EMAIL_ADDRESS ou EMAIL_PASSWORD não estão configuradas corretamente.");
            }

            try
            {
                _logger.LogInformation("Tentando descriptografar a senha do email...");
                _emailPassword = _cryptographyService.Decrypt(encryptedPassword).Replace(" ", ""); // Remove espaços
                _logger.LogInformation("Senha descriptografada com sucesso. Comprimento: {Length}, Primeiros caracteres: {FirstChars}",
                    _emailPassword.Length,
                    _emailPassword.Substring(0, Math.Min(4, _emailPassword.Length)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao descriptografar a senha do e-mail.");
                throw new ApplicationException("Falha na configuração do serviço de e-mail.", ex);
            }
        }

        public async Task SendEmailConfirmationAsync(string email, string confirmationToken, string name, string confirmationLink)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
            ArgumentException.ThrowIfNullOrEmpty(confirmationToken, nameof(confirmationToken));

            try
            {
                _logger.LogInformation("Preparando email de confirmação para {Name} ({Email})", name, email);

                var expirationTime = _jwtTokenService.GetExpirationTime();
                var expirationDays = Math.Ceiling((expirationTime - DateTime.UtcNow).TotalDays);

                confirmationLink = $"https://615c-2804-56c-a404-db00-8195-9526-fd59-1f33.ngrok-free.app/confirm-email?token={confirmationToken}";
                _logger.LogInformation("Link de confirmação gerado: {Link}", confirmationLink);

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

                _logger.LogInformation("Email de confirmação enviado com sucesso para {Name} ({Email})", name, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar email de confirmação para {Email}", email);
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
                _logger.LogInformation("Iniciando envio de email para {To}", to);
                _logger.LogInformation("Configurações SMTP: Server={Server}, Port={Port}, FromEmail={FromEmail}",
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    _emailSettings.FromEmail);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(to, to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                message.Body = bodyBuilder.ToMessageBody();
                _logger.LogInformation("Mensagem de email preparada com sucesso");

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    _logger.LogInformation("Configurando cliente SMTP...");
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                    _logger.LogInformation("Conectando ao servidor SMTP...");
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    _logger.LogInformation("Conexão SMTP estabelecida com sucesso");

                    _logger.LogInformation("Tentando autenticar com o email: {Email}", _emailSettings.FromEmail);
                    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailPassword);
                    _logger.LogInformation("Autenticação SMTP realizada com sucesso");

                    _logger.LogInformation("Enviando email...");
                    await client.SendAsync(message);
                    _logger.LogInformation("Email enviado com sucesso");

                    await client.DisconnectAsync(true);
                    _logger.LogInformation("Desconectado do servidor SMTP");
                }

                _logger.LogInformation("Email enviado com sucesso para {EmailAddress}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Detalhes do erro ao enviar email para {EmailAddress}: {ErrorMessage}", to, ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception: {InnerError}", ex.InnerException.Message);
                }
                throw new ApplicationException("Falha ao enviar email", ex);
            }
        }

        public async Task SendEventInviteAsync(string email, string userName, string eventDescription, DateTime startTime)
        {
            _logger.LogInformation("Preparando convite de evento para {UserName} ({Email})", userName, email);

            var subject = "Convite para Evento - EventosPro";
            var body = $@"
                <h2>Olá {userName},</h2>
                <p>Você foi convidado para o evento:</p>
                <h3>{eventDescription}</h3>
                <p>Data e Hora: {startTime:dd/MM/yyyy HH:mm}</p>
                <p>Por favor, acesse sua conta para responder ao convite.</p>";

            await SendEmailAsync(email, subject, body);
            _logger.LogInformation("Convite de evento enviado com sucesso para {UserName} ({Email})", userName, email);
        }
    }
}