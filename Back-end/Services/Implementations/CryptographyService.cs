using EventosPro.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz.Logging;
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

            string keyFromEnv = Environment.GetEnvironmentVariable("CRYPTO_KEY")
                         ?? throw new InvalidOperationException("Encryption key not configured in the environment.");

            _logger.LogInformation("Encryption key obtained from environment variables.");

            _key = Encoding.UTF8.GetBytes(keyFromEnv);

            if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
            {
                _logger.LogError("Invalid key size. The key must be 16, 24, or 32 bytes long.");
                throw new InvalidOperationException("The encryption key must be 16, 24, or 32 bytes (128, 192, or 256 bits).");
            }

            _logger.LogInformation("Encryption key validated successfully.");
        }

        public string Encrypt(string plainText) 
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    byte[] combinedData = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, combinedData, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combinedData, aes.IV.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combinedData);
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            _logger.LogInformation("Starting decryption...");

            try
            {
                encryptedText = encryptedText.Trim();

                // Converter a string Base64 para bytes
                byte[] combinedData = Convert.FromBase64String(encryptedText);

                if (combinedData.Length < 16)
                {
                    _logger.LogError("Encrypted data is too short to contain an IV.");
                    throw new CryptographicException("Insufficient data for decryption (missing IV).");
                }

                // Extrair IV (primeiros 16 bytes)
                byte[] iv = new byte[16];
                Buffer.BlockCopy(combinedData, 0, iv, 0, iv.Length);

                // Extrair o ciphertext (o restante dos bytes)
                byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];
                Buffer.BlockCopy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        _logger.LogInformation("Decryptor created.");
                        byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                        _logger.LogInformation("Decryption completed successfully.");
                        return Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during decryption.");
                throw;
            }
        }

        /*
        // Adicionar isso exatamente onde está 
        public void CriptografiaDaSenha()
        {
            Console.Write("Digite a senha para criptografar: ");
            string senhaOriginal = Console.ReadLine();

            if (string.IsNullOrEmpty(senhaOriginal))
            {
                Console.WriteLine("A senha não pode ser nula ou vazia.");
                return;
            }

            string senhaCriptografada = Encrypt(senhaOriginal);
            Console.WriteLine($"Senha criptografada: {senhaCriptografada}");
        }
        // Adicione isso logo após a linha var builder = WebApplication.CreateBuilder(args);
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<CryptographyService>();
        var cryptoService = new CryptographyService(logger);
        cryptoService.CriptografiaDaSenha();
        */
    }
}
