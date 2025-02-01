using EventosPro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EventosPro.Services.Interfaces;

namespace EventosPro.Services.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly ILogger<CookieService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(ILogger<CookieService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInWithCookieAsync(User user, bool rememberMe = false)
        {
            try
            {
                _logger.LogInformation("Starting the login process for user {UserId}.", user.Id);

                var claims = new List<Claim>()
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("IsConfirmed", user.IsConfirmed.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ?
                        DateTime.UtcNow.AddDays(30) :
                        DateTime.UtcNow.AddDays(25)
                };

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("Login successful for user {UserId}.", user.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while attempting to log in for user {UserId}.", user.Id);
                throw;
            }
        }

        public async Task SignOutFromCookieAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Starting the logout process.");
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while attempting to log out.");
                throw;
            }
        }
    }
}
