using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace EventosPro.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly ILogger<PasswordService> _logger;
        private readonly IUserRepository _userRepository;

        public PasswordService(ILogger<PasswordService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public string HashPassword(string password)
        {
            _logger.LogInformation("Starting password hashing.");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            _logger.LogInformation("Password hashing completed.");
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            _logger.LogInformation("Verifying password.");
            bool isValid = BCrypt.Net.BCrypt.Verify(password, passwordHash);

            _logger.LogInformation($"Password verification result: {isValid}");
            return isValid;
        }

        public bool ValidatePasswordStrength(string password)
        {

            if (string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Password validation failed: Password is empty or null.");
                return false;
            }
                
            var rules = new List<(Func<string, bool> Rule, string Message)>
            {
                (p => p.Length >= 8, "The password must be at least 8 characters long."),
                (p => p.Any(char.IsUpper), "The password must contain at least one uppercase letter."),
                (p => p.Any(char.IsLower), "The password must contain at least one lowercase letter."),
                (p => p.Any(char.IsDigit), "The password must contain at least one number."),
                (p => p.Any(c => !char.IsLetterOrDigit(c)), "The password must contain at least one special character.")
            };

            var failedRules = rules.Where(rule => !rule.Rule(password))
                                 .Select(rule => rule.Message)
                                 .ToList();

            if (failedRules.Any())
            {
                _logger.LogWarning($"Password validation failed: {string.Join(" ", failedRules)}");
                throw new ArgumentException(string.Join(" ", failedRules));
            }

            _logger.LogInformation("Password meets strength requirements.");
            return true;
        }

        public char GetRandomChar(string chars, RandomNumberGenerator rng)
        {
            _logger.LogDebug("Generating a random character from the provided set.");

            if (string.IsNullOrEmpty(chars))
            {
                _logger.LogWarning("Character set is empty or null.");
                throw new ArgumentException("Character set cannot be empty.");
            }

            byte[] randomBytes = new byte[1]; 
            char[] result = new char[1]; 

            do
            {
                rng.GetBytes(randomBytes); 
                result[0] = chars[randomBytes[0] % chars.Length];
            } while (!chars.Contains(result[0]));

            _logger.LogDebug($"Random character generated: {result[0]}");
            return result[0]; 
        }

        public string GenerateSecureRandomPassword()
        {
            try
            {
                _logger.LogInformation("Generating secure random password.");

                const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
                const string numberChars = "0123456789";
                const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

                using var rng = RandomNumberGenerator.Create();

                var password = new StringBuilder();

                password.Append(GetRandomChar(upperCaseChars, rng));  
                password.Append(GetRandomChar(lowerCaseChars, rng));  
                password.Append(GetRandomChar(numberChars, rng));     
                password.Append(GetRandomChar(specialChars, rng));    

                var allChars = upperCaseChars + lowerCaseChars + numberChars + specialChars;
                while (password.Length < 16) 
                {
                    password.Append(GetRandomChar(allChars, rng));
                }

                var finalPassword = password.ToString().ToCharArray();
                int n = finalPassword.Length;

                while (n > 1)
                {
                    byte[] box = new byte[1]; 
                    do rng.GetBytes(box); 
                    while (!(box[0] < n * (byte.MaxValue / n))); 
                    int k = (box[0] % n); 
                    n--; 
                    var value = finalPassword[k];
                    finalPassword[k] = finalPassword[n];
                    finalPassword[n] = value;
                }

                var result = new string(finalPassword);

                ValidatePasswordStrength(result);

                _logger.LogInformation("Secure random password generated successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating secure random password");
                throw;
            }
        }

        public bool ValidatePassword(string password, string storedHash)
        {
            try
            {
                _logger.LogInformation("Starting password validation..");

                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                {
                    _logger.LogWarning("Attempt to validate with a null password or hash.");
                    return false;
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

                _logger.LogInformation($"Password validation completed. Result: {isValid}");
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password validation.");
                return false;
            }
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                _logger.LogInformation($"Starting password update for user {userId}.");

                if (!ValidatePasswordStrength(newPassword))
                {
                    return false;
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"Attempt to update password for non-existent user. ID: {userId}");
                    return false;
                }

                if (!ValidatePassword(currentPassword, user.PasswordHash))
                {
                    _logger.LogWarning($"Attempt to update password with incorrect current password. UserID: {userId}");
                    return false;
                }

                var newPasswordHash = HashPassword(newPassword);

                user.PasswordHash = newPasswordHash;
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation($"Password successfully updated for the user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during user password update {userId}");
                throw;
            }
        }
    }
}
