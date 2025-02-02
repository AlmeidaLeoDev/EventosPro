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

        public async Task<IEnumerable<User>> GetUnconfirmedUsersAsync()
        {
            return await _dbSet
                .Where(u => !u.IsConfirmed)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersWithExpiredTokensAsync()
        {
            var now = DateTime.UtcNow; // Usar UTC para consistência

            return await _dbSet
                .Where(u =>
                    (u.EmailConfirmationTokenExpires != null && u.EmailConfirmationTokenExpires < now) ||
                    (u.PasswordResetTokenExpires != null && u.PasswordResetTokenExpires < now)
                )
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await _dbSet.FindAsync(id);
            if (user != null)
            {
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
