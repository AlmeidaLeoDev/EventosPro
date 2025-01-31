using EventosPro.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace EventosPro.Services.Implementations
{
    public class CryptographyService : ICryptographyService
    {
        private readonly byte[] _key;
        private readonly ILogger<CryptographyService> _logger;

        public CryptographyService(ILogger<CryptographyService> logger)
        {
            _logger = logger;

            _logger.LogInformation("Initializing CryptographyService...");

            string key = Environment.GetEnvironmentVariable("CRYPTO_KEY")
                         ?? throw new InvalidOperationException("Encryption key not configured in the environment.");

            _logger.LogInformation("Encryption key obtained from environment variables.");

            _key = Encoding.UTF8.GetBytes(key);

            if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
            {
                _logger.LogError("Invalid key size. The key must be 16, 24, or 32 bytes long.");
                throw new InvalidOperationException("The encryption key must be 16, 24, or 32 bytes (128, 192, or 256 bits).");
            }

            _logger.LogInformation("Encryption key validated successfully.");
        }

        public string Encrypt(string plainText)
        {
            _logger.LogInformation("Starting encryption...");

            using (Aes aes = Aes.Create())
            {
                _logger.LogInformation("AES instance created.");

                aes.Key = _key;
                _logger.LogInformation("AES key set.");

                aes.GenerateIV();
                _logger.LogInformation("IV generated"); 

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    _logger.LogInformation("Encryptor created.");

                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    byte[] combinedData = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, combinedData, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combinedData, aes.IV.Length, encryptedBytes.Length);

                    _logger.LogInformation("Encryption completed successfully.");
                    return Convert.ToBase64String(combinedData);
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            _logger.LogInformation("Starting decryption...");

            using (Aes aes = Aes.Create())
            {
                _logger.LogInformation("AES instance created.");

                byte[] combinedData = Convert.FromBase64String(encryptedText);

                byte[] iv = new byte[16]; 
                Buffer.BlockCopy(combinedData, 0, iv, 0, iv.Length);

                byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];
                Buffer.BlockCopy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                aes.Key = _key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    _logger.LogInformation("Decryptor created.");

                    byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                    _logger.LogInformation("Decryption completed successfully.");
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }

    }
}
