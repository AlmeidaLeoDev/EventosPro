using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EventosPro.Services.Implementations
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IGmailService _gmailService; 
        private readonly IPasswordService _passwordService;
        private readonly ILogger<PasswordResetService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private const int TOKEN_EXPIRATION_DAYS = 1;
        private readonly string _baseUrl;
        private readonly IConfiguration _configuration;

        public PasswordResetService(IUserRepository userRepository, IEmailService emailService, IGmailService gmailService, IPasswordService passwordService, ILogger<PasswordResetService> logger, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _gmailService = gmailService;
            _passwordService = passwordService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _baseUrl = _configuration.GetValue<string>("AppSettings:BaseUrl");
            _configuration = configuration;
        }


        public async Task InitiatePasswordResetAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Attempt to reset password for unregistered email: {Email}", email);
                    return;
                }

                var token = await _jwtTokenService.GenerateJwtTokenAsync(user);

                user.PasswordResetToken = token;
                user.PasswordResetTokenExpires = _jwtTokenService.GetExpirationTime();

                await _userRepository.UpdateAsync(user);

                string resetLink = $"{_baseUrl.TrimEnd('/')}/reset-password?token={token}&email={Uri.EscapeDataString(email)}";
                string emailBody = $@"
                <html>
                <head>
                    <meta charset='UTF-8'>
                </head>
                <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2>Redefinição de Senha - EventosPro</h2>
                    <p>Olá {user.Name},</p>
                    <p>Recebemos uma solicitação para redefinir sua senha no EventosPro.</p>
                    <p>Se você não solicitou esta alteração, ignore este e-mail.</p>
                    <p>Para redefinir sua senha, clique no link abaixo (valido por {TOKEN_EXPIRATION_DAYS} dia):</p>
                    <p><a href='{resetLink}'>Redefinir Senha</a></p>
                    <p>Por segurança, este link expirará em {TOKEN_EXPIRATION_DAYS} dia.</p>
                </body>
                </html > ";

                await _gmailService.SendEmailAsync(
                    email,
                    "Redefinicao de Senha - EventosPro",
                    emailBody
                );

                _logger.LogInformation("Password reset email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing password reset request for {Email}", email);
                throw;
            }
        }

        public async Task<bool> ValidateResetTokenAsync(string token)
        {
            try
            {
                var (isValid, email) = _jwtTokenService.ValidateTokenAndGetEmail(token);

                if (!isValid)
                {
                    _logger.LogWarning("Token inválido durante validação de reset de senha");
                    return false;
                }

                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Usuário não encontrado durante validação de reset de senha");
                    return false;
                }

                if (user.PasswordResetToken != token)
                {
                    _logger.LogWarning("Token de reset não corresponde ao token armazenado");
                    return false;
                }

                if (_jwtTokenService.IsTokenExpired(user.PasswordResetTokenExpires ?? DateTime.MinValue))
                {
                    _logger.LogWarning("Token de reset de senha expirado para o usuário {Email}", email);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar token de reset de senha");
                throw;
            }
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                if (!await ValidateResetTokenAsync(token))
                {
                    throw new InvalidOperationException("Invalid or expired token.");
                }

                // Criptografa a nova senha
                user.PasswordHash = _passwordService.HashPassword(newPassword);

                // Limpa o token de reset
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpires = null;

                await _userRepository.UpdateAsync(user);

                // Envia confirmação por email
                string emailBody = @"
                <html>
                <head>
                    <meta charset='UTF-8'>
                </head>
                <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2>Senha Alterada com Sucesso</h2>
                    <p>Sua senha foi alterada com sucesso no EventosPro.</p>
                    <p>Se você não realizou esta alteração, entre em contato conosco imediatamente.</p>
                </body>
                </html> ";

                await _gmailService.SendEmailAsync(
                    email,
                    "Confirmacao de Alteracao de Senha - EventosPro",
                    emailBody
                );

                _logger.LogInformation("Password reset successfully for {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when resetting password for {Email}", email);
                throw;
            }
        }

    }
}
