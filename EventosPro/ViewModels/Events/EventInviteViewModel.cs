namespace EventosPro.ViewModels.Events
{
    public class EventInviteViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResponseAt { get; set; }
        public int? EventId { get; set; }
        public int? InvitedUserId { get; set; }
        public string InvitedUserName { get; set; }
        public string InvitedUserEmail { get; set; }
    }
}
