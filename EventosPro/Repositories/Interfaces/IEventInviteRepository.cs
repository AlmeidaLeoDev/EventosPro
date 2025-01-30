using EventosPro.Models;

namespace EventosPro.Repositories.Interfaces
{
    public interface IEventInviteRepository : IRepository<EventInvite>
    {
        Task<IEnumerable<EventInvite>> GetUserInvitesAsync(int userId);
        Task<EventInvite> GetInviteAsync(int eventId, int userId);
    }
}
