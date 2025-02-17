namespace EventosPro.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailConfirmationAsync(string email, string confirmationToken, string name);
        public Task SendEventInviteAsync(string email, string userName, string eventDescription, DateTime startTime);

    }
}
