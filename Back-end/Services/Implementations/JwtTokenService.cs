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

        public (bool IsValid, string Email) ValidateTokenAndGetEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is null or empty.");
                return (false, string.Empty);
            }

            _logger.LogInformation("Validating JWT token and extracting email...");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            try
            {
                // Valida o token e obtém os claims
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Extrai o email dos claims
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Token is valid but doesn't contain email claim.");
                    return (false, string.Empty);
                }

                _logger.LogInformation("JWT token is valid and email was extracted successfully.");
                return (true, email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT token validation failed.");
                return (false, string.Empty);
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
