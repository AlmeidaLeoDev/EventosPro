using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Events
{
    public class CreateEventInviteViewModel
    {
        public int EventId { get; set; }

        public string InvitedUserEmail { get; set; }
    }
}
