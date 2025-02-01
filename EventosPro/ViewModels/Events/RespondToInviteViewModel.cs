using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Events
{
    public class RespondToInviteViewModel
    {
        public int InviteId { get; set; }

        public string Response { get; set; }
    }
}
