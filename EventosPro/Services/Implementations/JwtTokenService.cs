using EventosPro.Models;
using EventosPro.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventosPro.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task<string> GenerateJwtTokenAsync(User user)
        {
            _logger.LogInformation("Generating JWT token for user {UserId} ({UserName})", user.Id, user.Name);
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("IsConfirmed", user.IsConfirmed.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("JWT token successfully generated for user {UserId} ({UserName})", user.Id, user.Name);

            return Task.FromResult(tokenString);
        }

        public bool ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is null or empty.");
                return false;
            }

            _logger.LogInformation("Validating JWT token...");

            var tokenHandler = new JwtSecurityTokenHandler(); 
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(key), 
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ClockSkew = TimeSpan.Zero 
                },
                    out SecurityToken validatedToken);

                _logger.LogInformation("JWT token is valid.");
                return true; 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "JWT token validation failed.");
                return false;
            }
        }

        public DateTime GetExpirationTime() 
        {
            var expirationTime = DateTime.UtcNow.AddDays(1);
            _logger.LogInformation("JWT token expiration time is {ExpirationTime}", expirationTime);
            return expirationTime;
        }

        public bool IsTokenExpired(DateTime expirationTime) 
        {
            bool isExpired = expirationTime < DateTime.UtcNow;

            _logger.LogInformation("Token expired check: {IsExpired}", isExpired);
            return isExpired;
        }

    }
}
