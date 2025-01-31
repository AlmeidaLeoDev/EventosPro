using System.Security.Cryptography;

namespace EventosPro.Services.Interfaces
{
    public interface IPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
        public bool ValidatePasswordStrength(string password);
        public char GetRandomChar(string chars, RandomNumberGenerator rng);
        public string GenerateSecureRandomPassword();
        public bool ValidatePassword(string password, string storedHash);
        public Task<bool> UpdatePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
