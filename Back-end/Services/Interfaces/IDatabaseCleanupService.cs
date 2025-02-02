namespace EventosPro.Services.Interfaces
{
    public interface IDatabaseCleanupService
    {
        Task CleanupUnconfirmedUsersAsync();
        Task CleanupExpiredTokensAsync();
    }
}
