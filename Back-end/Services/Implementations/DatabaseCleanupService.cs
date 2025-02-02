using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;

namespace EventosPro.Services.Implementations
{
    public class DatabaseCleanupService : IDatabaseCleanupService
    {
        private readonly ILogger<DatabaseCleanupService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public DatabaseCleanupService(ILogger<DatabaseCleanupService> logger, IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task CleanupUnconfirmedUsersAsync()
        {
            try
            {
                var unconfirmedUsers = await _userRepository.GetUnconfirmedUsersAsync();

                var expiredUsers = unconfirmedUsers.Where(u =>
                    !u.IsConfirmed &&
                    _jwtTokenService.IsTokenExpired(u.EmailConfirmationTokenExpires ?? DateTime.MinValue)
                );

                foreach (var user in expiredUsers)
                {
                    try
                    {
                        user.EmailConfirmationToken = null;
                        user.EmailConfirmationTokenExpires = null;
                        user.PasswordResetToken = null;
                        user.PasswordResetTokenExpires = null;

                        await _userRepository.DeleteByIdAsync(user.Id);
                        _logger.LogInformation(
                            "Unconfirmed user removed: Name: {Name}, {Email}, (ID: {UserId}, Created on: {CreatedAt})",
                            user.Name,
                            user.Email,
                            user.Id,
                            user.CreatedAt
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error removing unconfirmed user: {Email}",
                            user.Email
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when cleaning unconfirmed users");
                throw;
            }
        }

        public async Task CleanupExpiredTokensAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersWithExpiredTokensAsync();
                var usersWithExpiredTokens = users.Where(u =>
                    (_jwtTokenService.IsTokenExpired(u.EmailConfirmationTokenExpires ?? DateTime.MinValue) && !string.IsNullOrEmpty(u.EmailConfirmationToken)) ||
                    (_jwtTokenService.IsTokenExpired(u.PasswordResetTokenExpires ?? DateTime.MinValue) && !string.IsNullOrEmpty(u.PasswordResetToken))
                );

                foreach (var user in usersWithExpiredTokens)
                {
                    try
                    {
                        if (_jwtTokenService.IsTokenExpired(user.EmailConfirmationTokenExpires ?? DateTime.MinValue))
                        {
                            user.EmailConfirmationToken = null;
                            user.EmailConfirmationTokenExpires = null;
                        }

                        if (_jwtTokenService.IsTokenExpired(user.PasswordResetTokenExpires ?? DateTime.MinValue))
                        {
                            user.PasswordResetToken = null;
                            user.PasswordResetTokenExpires = null;
                        }

                        await _userRepository.UpdateAsync(user);
                        _logger.LogInformation(
                            "Expired tokens removed for user: {Email}",
                            user.Email
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error clearing user's expired tokens: {Email}",
                            user.Email
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing expired tokens");
                throw;
            }
        }

    }
}
