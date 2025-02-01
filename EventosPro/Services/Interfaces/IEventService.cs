using EventosPro.Models;

namespace EventosPro.Services.Interfaces
{
    public interface IEventService
    {
        public Task<Event> CreateEventAsync(Event eventEntity, int userId);
        public Task<Event> UpdateEventAsync(Event eventEntity, int userId);
        public Task DeleteEventAsync(int eventId, int userId);
        public Task<IEnumerable<Event>> GetUserEventsAsync(int userId);
        public Task<Event> GetEventByIdAsync(int eventId, int userId);
        public Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime start, DateTime end, int userId);
        public Task<EventInvite> InviteUserToEventAsync(int eventId, int invitedUserId, int currentUserId);
        public Task<EventInvite> RespondToInviteAsync(int inviteId, InviteStatus response, int userId);
    }
}
