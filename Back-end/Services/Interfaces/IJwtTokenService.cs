using EventosPro.Models;

namespace EventosPro.Services.Interfaces
{
    public interface IJwtTokenService
    {
        public Task<string> GenerateJwtTokenAsync(User user);
        public (bool IsValid, string Email) ValidateTokenAndGetEmail(string token);
        public DateTime GetExpirationTime();
        public bool IsTokenExpired(DateTime expirationTime);
    }
}
