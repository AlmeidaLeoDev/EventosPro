using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventosPro.Models
{
    public class EventInvite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public InviteStatus Status { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public DateTime? ResponseAt { get; set; }

        [ForeignKey("Event")]
        public int? EventId { get; set; }

        public Event Event { get; set; }

        [ForeignKey("InvitedUser")]
        public int? InvitedUserId { get; set; }

        public User InvitedUser { get; set; }
    }

    public enum InviteStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
