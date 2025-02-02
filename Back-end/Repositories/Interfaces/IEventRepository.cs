using EventosPro.Models;

namespace EventosPro.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetUserEventsAsync(int userId);
        Task<bool> HasTimeConflictAsync(DateTime startTime, DateTime endTime, int userId, int? excludeEventId = null);
        Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime startDate, DateTime endDate, int userId);
    }
}
