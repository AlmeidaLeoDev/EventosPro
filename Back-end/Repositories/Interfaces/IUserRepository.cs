﻿using EventosPro.Models;

namespace EventosPro.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetUnconfirmedUsersAsync();
        Task<IEnumerable<User>> GetUsersWithExpiredTokensAsync();
        Task DeleteByIdAsync(int id);
    }
}
