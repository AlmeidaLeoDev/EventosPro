namespace EventosPro.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailConfirmationAsync(string email, string confirmationToken, string name, string confirmationLink);
        public Task SendEmailAsync(string to, string subject, string body);
    }
}
