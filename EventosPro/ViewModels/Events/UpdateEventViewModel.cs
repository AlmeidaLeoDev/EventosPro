using System.ComponentModel.DataAnnotations;

namespace EventosPro.ViewModels.Events
{
    public class UpdateEventViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
