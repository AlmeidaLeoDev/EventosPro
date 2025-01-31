namespace EventosPro.Services.Interfaces
{
    public interface ICryptographyService
    {
        public string Encrypt(string plainText);
        public string Decrypt(string encryptedText);
    }
}
