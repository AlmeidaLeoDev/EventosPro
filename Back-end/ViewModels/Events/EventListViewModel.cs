namespace EventosPro.ViewModels.Events
{
    public class EventListViewModel
    {
        public List<EventDetailsViewModel> Events { get; set; }
        public List<EventInviteViewModel> PendingInvites { get; set; }
    }
}
