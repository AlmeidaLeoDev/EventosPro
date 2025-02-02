using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;

namespace EventosPro.Services.Implementations
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        private readonly ICryptographyService _cryptographyService;
        private readonly ILogger<PasswordResetService> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        private const int TOKEN_EXPIRATION_DAYS = 1;

        public PasswordResetService(IUserRepository userRepository, IEmailService emailService, IPasswordService passwordService, ICryptographyService cryptographyService, ILogger<PasswordResetService> logger, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _passwordService = passwordService;
            _cryptographyService = cryptographyService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
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

                var token = _jwtTokenService.GenerateJwtTokenAsync(user);

                user.PasswordResetToken = await token;
                user.PasswordResetTokenExpires = _jwtTokenService.GetExpirationTime();

                await _userRepository.UpdateAsync(user);

                string resetLink = $"https://yourdomain.com/reset-password?token={token}&email={Uri.EscapeDataString(email)}";
                string emailBody = $@"
                    <h2>Redefinição de Senha - EventosPro</h2>
                    <p>Olá {user.Name},</p>
                    <p>Recebemos uma solicitação para redefinir sua senha no EventosPro.</p>
                    <p>Se você não solicitou esta alteração, ignore este e-mail.</p>
                    <p>Para redefinir sua senha, clique no link abaixo (válido por {TOKEN_EXPIRATION_DAYS} dia):</p>
                    <p><a href='{resetLink}'>Redefinir Senha</a></p>
                    <p>Por segurança, este link expirará em {TOKEN_EXPIRATION_DAYS} dia.</p>";

                await _emailService.SendEmailAsync(
                    email,
                    "Redefinição de Senha - EventosPro",
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

        public async Task<bool> ValidateResetTokenAsync(string email, string token)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    return false;
                }

                return _jwtTokenService.ValidateJwtToken(token) &&
                       !_jwtTokenService.IsTokenExpired(user.PasswordResetTokenExpires ?? DateTime.MinValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating reset token for {Email}", email);
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

                if (!await ValidateResetTokenAsync(email, token))
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
                    <h2>Senha Alterada com Sucesso</h2>
                    <p>Sua senha foi alterada com sucesso no EventosPro.</p>
                    <p>Se você não realizou esta alteração, entre em contato conosco imediatamente.</p>";

                await _emailService.SendEmailAsync(
                    email,
                    "Confirmação de Alteração de Senha - EventosPro",
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
