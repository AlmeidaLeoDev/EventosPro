using EventosPro.Models;

namespace EventosPro.Services.Interfaces
{
    public interface ICookieService
    {
        public Task SignInWithCookieAsync(User user, bool rememberMe = false);
        public Task SignOutFromCookieAsync(HttpContext context);
    }
}
