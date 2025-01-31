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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Events (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Events)
                .WithOne(e => e.EventUser)
                .HasForeignKey(e => e.EventUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Event -> EventInvites (1:N)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventInvites)
                .WithOne(ei => ei.Event)
                .HasForeignKey(ei => ei.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            // User -> EventInvites (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.EventInvites)
                .WithOne(ei => ei.InvitedUser)
                .HasForeignKey(ei => ei.InvitedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Settings
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<EventInvite>()
                .HasIndex(ei => new { ei.EventId, ei.InvitedUserId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(p => p.IsConfirmed)
                .HasDefaultValue(false);
        }
    }
}
