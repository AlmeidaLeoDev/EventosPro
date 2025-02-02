using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventosPro.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public EventStatus Status { get; set; }

        [NotMapped]
        public bool IsLongDuration => EndTime.Date > StartTime.Date;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        [ForeignKey("EventUser")]
        public int EventUserId { get; set; }

        public User EventUser { get; set; }

        public ICollection<EventInvite> EventInvites { get; set; }
    }

    public enum EventStatus
    {
        Active,
        Completed,
        Cancelled
    }
}
