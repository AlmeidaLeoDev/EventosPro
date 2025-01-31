using EventosPro.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EventosPro.Services.Implementations
{
    public class CryptographyService : ICryptographyService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public CryptographyService(IConfiguration configuration)
        {
            _key = Encoding.UTF8.GetBytes(configuration["Cryptography:Key"]);
            _iv = Encoding.UTF8.GetBytes(configuration["Cryptography:IV"]);
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encryptedText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
