namespace EventosPro.ViewModels.Events
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public bool IsLongDuration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int EventUserId { get; set; }
        public string EventUserName { get; set; }
        public List<EventInviteViewModel> Invites { get; set; } 
    }
}
