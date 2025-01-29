using EventosPro.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosPro.Context
{
    public class ApplicationDbContex : DbContext 
    {
        public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventInvite> Invites { get; set; }
    }
}
