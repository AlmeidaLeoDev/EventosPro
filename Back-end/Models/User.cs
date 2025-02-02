using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventosPro.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public string PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpires { get; set; }

        public string EmailConfirmationToken { get; set; }

        public DateTime? EmailConfirmationTokenExpires { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public DateTime? ConfirmedAt { get; set; }

        public string ConfirmationToken { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [StringLength(100)]
        public string ResetPasswordToken { get; set; }

        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventInvite> EventInvites { get; set; }
    }
}
