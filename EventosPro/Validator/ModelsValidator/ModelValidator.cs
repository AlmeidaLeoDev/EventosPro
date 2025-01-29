using EventosPro.Context;
using EventosPro.Models;
using Microsoft.EntityFrameworkCore;

namespace EventosPro.Validator.ModelsValidator
{
    public class ModelValidator
    {
        protected void ModelValidations(ModelBuilder modelBuilder)
        {
            // Settings User -> Events (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Events)
                .WithOne(e => e.EventUser)
                .HasForeignKey(e => e.EventUserId)
                .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, their events are deleted

            // Settings Event -> EventInvites (1:N)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventInvites)
                .WithOne(ei => ei.Event)
                .HasForeignKey(ei => ei.EventId)
                .OnDelete(DeleteBehavior.SetNull); // If an event is deleted, its invitations are null

            // Settings User -> EventInvites (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.EventInvites)
                .WithOne(ei => ei.InvitedUser)
                .HasForeignKey(ei => ei.InvitedUserId)
                .OnDelete(DeleteBehavior.SetNull); // If a user is deleted, their received invites are null

            // Settings de unicidade
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<EventInvite>()
                .HasIndex(ei => new { ei.EventId, ei.InvitedUserId })
                .IsUnique(); // Prevents duplicate invitations
        }
    }
}
