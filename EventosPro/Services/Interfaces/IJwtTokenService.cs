using EventosPro.Models;

namespace EventosPro.Services.Interfaces
{
    public interface IJwtTokenService
    {
        public Task<string> GenerateJwtTokenAsync(User user);
        public bool ValidateJwtToken(string token);
        public DateTime GetExpirationTime();
        public bool IsTokenExpired(DateTime expirationTime);
    }
}
