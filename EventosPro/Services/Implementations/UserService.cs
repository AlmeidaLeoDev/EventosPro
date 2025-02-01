using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventosPro.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IPasswordService passwordService, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _passwordService = passwordService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task DeleteUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (user.Id <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(user));
            }

            try
            {
                await _userRepository.DeleteAsync(user.Id);
                _logger.LogInformation("User {UserId} successfully deleted", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", user.Id);
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("The email cannot be null or empty.", nameof(email));
            }
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                _logger.LogInformation("User {Email} {Status}",
                    email, user != null ? "found" : "not found");

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when searching for user by email {Email}", email);
                throw;
            }
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("The email cannot be null or empty.", nameof(email));
            }

            try
            {
                return await _userRepository.EmailExistsAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email record {Email}", email);
                throw;
            }
        }

        public async Task ConfirmEmailAsync(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Email or token cannot be empty or null.");
            }

            try
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (user.IsConfirmed)
                {
                    _logger.LogInformation("Email already confirmed for user {Email}", email);
                    return;
                }

                if (!_jwtTokenService.ValidateJwtToken(token) ||
                     _jwtTokenService.IsTokenExpired(user.EmailConfirmationTokenExpires ?? DateTime.MinValue))
                {
                    throw new InvalidOperationException("Invalid or expired confirmation token.");
                }

                user.IsConfirmed = true;
                user.ConfirmedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("Successfully confirmed email for user {Email}", email);

            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error confirming email {Email}", email);
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            try
            {
                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("User {UserId} updated successfully", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", user.Id);
                throw;
            }
        }

        public async Task AddUserAsync(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("User email cannot be empty.", nameof(user));
            }

            try
            {
                bool isEmailRegistered = await IsEmailRegisteredAsync(user.Email);
                if (isEmailRegistered)
                {
                    _logger.LogWarning("Attempt to register with an existing email: {Email}", user.Email);
                    throw new InvalidOperationException("Email already registered.");
                }

                var hashedPassword = _passwordService.HashPassword(password);
                user.PasswordHash = hashedPassword;

                user.CreatedAt = DateTime.UtcNow;
                user.IsConfirmed = false;
                user.ConfirmationToken = await _jwtTokenService.GenerateJwtTokenAsync(user);
                user.EmailConfirmationTokenExpires = _jwtTokenService.GetExpirationTime();

                await _userRepository.AddAsync(user);
                _logger.LogInformation("New user successfully registered: {Email}", user.Email);
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "Error when adding new user with email {Email}", user.Email);
                throw;
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"User with email {email} not found.");
                    return false;
                }

                return _passwordService.VerifyPassword(password, user.PasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating credentials for email {Email}.", email);
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                _logger.LogInformation("User with ID {UserId} fetched successfully.", userId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID: {UserId}", userId);
                throw;
            }
        }
    }

}
