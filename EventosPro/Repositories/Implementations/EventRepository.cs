using EventosPro.Context;
using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventosPro.Repositories.Implementations
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContex context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetUserEventsAsync(int userId)
        {
            return await _dbSet
                .Where(e => e.EventUserId == userId && e.Status == EventStatus.Active)
                .Include(e => e.EventUser)
                .Include(e => e.EventInvites)
                    .ThenInclude(ei => ei.InvitedUser)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<bool> HasTimeConflictAsync(DateTime startTime, DateTime endTime, int userId, int? excludeEventId = null)
        {
            var query = _dbSet.Where(e =>
                e.EventUserId == userId &&
                e.Status == EventStatus.Active &&
                ((e.StartTime <= startTime && e.EndTime > startTime) ||
                (e.StartTime < endTime && e.EndTime >= endTime) ||
                (e.StartTime >= startTime && e.EndTime <= endTime)));

            if (excludeEventId.HasValue)
            {
                query = query.Where(e => e.Id != excludeEventId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime startDate, DateTime endDate, int userId)
        {
            return await _dbSet
                .Where(e => e.EventUserId == userId &&
                           e.StartTime >= startDate &&
                           e.EndTime <= endDate &&
                           e.Status == EventStatus.Active)
                .Include(e => e.EventUser)
                .Include(e => e.EventInvites)
                    .ThenInclude(ei => ei.InvitedUser)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public override async Task<Event> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(e => e.EventUser)
                .Include(e => e.EventInvites)
                    .ThenInclude(ei => ei.InvitedUser)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
