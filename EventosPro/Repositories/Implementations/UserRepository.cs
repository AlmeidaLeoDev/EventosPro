using EventosPro.Context;
using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventosPro.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository 
    {
        public UserRepository(ApplicationDbContex context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Events)
                .Include(u => u.EventInvites)
                    .ThenInclude(ei => ei.Event)
                .FirstOrDefaultAsync(u => u.Email == email && u.EmailConfirmed);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Events)
                .Include(u => u.EventInvites)
                    .ThenInclude(ei => ei.Event)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.Events)
                .Include(u => u.EventInvites)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}
