using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Events
{
    public class CreateEventViewModel
    {
        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
