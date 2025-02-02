namespace EventosPro.Services.Interfaces
{
    public interface IPasswordResetService
    {
        public Task InitiatePasswordResetAsync(string email);
        public Task<bool> ValidateResetTokenAsync(string email, string token);
        public Task ResetPasswordAsync(string email, string token, string newPassword);
    }
}
