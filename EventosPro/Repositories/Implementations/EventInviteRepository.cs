using EventosPro.Context;
using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventosPro.Repositories.Implementations
{
    public class EventInviteRepository : Repository<EventInvite>, IEventInviteRepository
    {
        public EventInviteRepository(ApplicationDbContex context) : base(context)
        {
        }

        public async Task<IEnumerable<EventInvite>> GetUserInvitesAsync(int userId)
        {
            return await _dbSet
                .Where(ei => ei.InvitedUserId == userId)
                .Include(ei => ei.Event)
                    .ThenInclude(e => e.EventUser)
                .Include(ei => ei.InvitedUser)
                .OrderBy(ei => ei.Event.StartTime)
                .ToListAsync();
        }

        public async Task<EventInvite> GetInviteAsync(int eventId, int userId)
        {
            return await _dbSet
                .Include(ei => ei.Event)
                    .ThenInclude(e => e.EventUser)
                .Include(ei => ei.InvitedUser)
                .FirstOrDefaultAsync(ei =>
                    ei.EventId == eventId &&
                    ei.InvitedUserId == userId);
        }

        public override async Task<EventInvite> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(ei => ei.Event)
                    .ThenInclude(e => e.EventUser)
                .Include(ei => ei.InvitedUser)
                .FirstOrDefaultAsync(ei => ei.Id == id);
        }

        public override async Task<IEnumerable<EventInvite>> GetAllAsync()
        {
            return await _dbSet
                .Include(ei => ei.Event)
                    .ThenInclude(e => e.EventUser)
                .Include(ei => ei.InvitedUser)
                .OrderBy(ei => ei.CreatedAt)
                .ToListAsync();
        }
    }
}
