using EventosPro.Models;

namespace EventosPro.Services.Interfaces
{
    public interface IUserService
    {
        public Task DeleteUserAsync(User user);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<bool> IsEmailRegisteredAsync(string email);
        public Task ConfirmEmailAsync(string token);
        public Task UpdateUserAsync(User user);
        public Task AddUserAsync(User user, string password);
        public Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<User> GetUserByIdAsync(int userId);

    }
}
