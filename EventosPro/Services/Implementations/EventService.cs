using EventosPro.Models;
using EventosPro.Repositories.Interfaces;
using EventosPro.Services.Interfaces;

namespace EventosPro.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventInviteRepository _eventInviteRepository;
        private readonly ILogger<EventService> _logger;

        public EventService(
            IEventRepository eventRepository,
            IEventInviteRepository eventInviteRepository,
            IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _eventInviteRepository = eventInviteRepository;
        }

        public async Task<Event> CreateEventAsync(Event eventEntity, int userId)
        {
            _logger.LogInformation("User {UserId} is attempting to create an event.", userId);

            if (eventEntity.StartTime >= eventEntity.EndTime)
            {
                _logger.LogWarning("User {UserId} provided an invalid event time range.", userId);
                throw new ArgumentException("The end time must be later than the start time");
            }

            var hasConflict = await _eventRepository.HasTimeConflictAsync(
                eventEntity.StartTime,
                eventEntity.EndTime,
                userId);

            if (hasConflict)
            {
                _logger.LogWarning("User {UserId} has a scheduling conflict when creating an event.", userId);
                throw new InvalidOperationException("There is a time conflict with another event");
            }

            eventEntity.EventUserId = userId;
            eventEntity.Status = EventStatus.Active;
            eventEntity.CreatedAt = DateTime.UtcNow;

            _logger.LogInformation("Event created successfully");
            return await _eventRepository.AddAsync(eventEntity);
        }

        public async Task<Event> UpdateEventAsync(Event eventEntity, int userId)
        {
            _logger.LogInformation("User is attempting to update event.");

            var existingEvent = await _eventRepository.GetByIdAsync(eventEntity.Id);

            if (existingEvent == null || existingEvent.EventUserId != userId)
            {
                _logger.LogWarning("Unauthorized update attempt by user on event.");
                throw new UnauthorizedAccessException("You do not have permission to edit this event");
            }

            if (eventEntity.StartTime >= eventEntity.EndTime)
            {
                _logger.LogWarning("User {UserId} provided an invalid event time range for event.", userId);
                throw new ArgumentException("The end time must be later than the start time");
            }

            var hasConflict = await _eventRepository.HasTimeConflictAsync(
                eventEntity.StartTime,
                eventEntity.EndTime,
                userId,
                eventEntity.Id);

            if (hasConflict)
            {
                _logger.LogWarning("User {UserId} has a scheduling conflict when updating event.", userId);
                throw new InvalidOperationException("There is a time conflict with another event");
            }

            existingEvent.Description = eventEntity.Description;
            existingEvent.StartTime = eventEntity.StartTime;
            existingEvent.EndTime = eventEntity.EndTime;
            existingEvent.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Event {EventId} successfully updated by user {UserId}.", eventEntity.Id, userId);

            await _eventRepository.UpdateAsync(existingEvent);
            return existingEvent;
        }

        public async Task DeleteEventAsync(int eventId, int userId)
        {
            _logger.LogInformation("User {UserId} is attempting to delete event {EventId}.", userId, eventId);

            var eventEntity = await _eventRepository.GetByIdAsync(eventId);

            if (eventEntity == null || eventEntity.EventUserId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt by user {UserId} on event {EventId}.", userId, eventId);
                throw new UnauthorizedAccessException("You do not have permission to delete this event");
            }

            eventEntity.Status = EventStatus.Cancelled;
            eventEntity.UpdatedAt = DateTime.UtcNow;
            await _eventRepository.UpdateAsync(eventEntity);

            _logger.LogInformation("Event {EventId} successfully deleted by user {UserId}.", eventId, userId);
        }

        public async Task<IEnumerable<Event>> GetUserEventsAsync(int userId)
        {
            _logger.LogInformation("Fetching events for user {UserId}.", userId);
            return await _eventRepository.GetUserEventsAsync(userId);
        }

        public async Task<Event> GetEventByIdAsync(int eventId, int userId)
        {
            _logger.LogInformation("User {UserId} is attempting to fetch event {EventId}.", userId, eventId);
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);

            if (eventEntity == null || eventEntity.EventUserId != userId)
            {
                _logger.LogWarning("Unauthorized access attempt by user {UserId} on event {EventId}.", userId, eventId);
                throw new UnauthorizedAccessException("You do not have permission to view this event");
            }

            _logger.LogInformation("Authorized access.");
            return eventEntity;
        }

        public async Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime start, DateTime end, int userId)
        {
            _logger.LogInformation("Fetching events for user {UserId} in range {Start} to {End}", userId, start, end);
            try
            {
                var events = await _eventRepository.GetEventsInRangeAsync(start, end, userId);
                var invites = await _eventInviteRepository.GetUserInvitesAsync(userId);

                var allEvents = new List<Event>();
                allEvents.AddRange(events);
                allEvents.AddRange(invites
                    .Where(i => i.Status == InviteStatus.Accepted && i.Event.Status == EventStatus.Active)
                    .Select(i => i.Event));

                _logger.LogInformation("Returning {TotalEventCount} events for user {UserId}", allEvents.Count, userId);
                return allEvents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while fetching events for user {UserId} in range {Start} to {End}",userId, start, end); 
                throw;
            }

        }

        public async Task<EventInvite> InviteUserToEventAsync(int eventId, int invitedUserId, int currentUserId)
        {
            _logger.LogInformation("User {UserId} is inviting user {InvitedUserId} to event {EventId}.", currentUserId, invitedUserId, eventId);

            var eventEntity = await _eventRepository.GetByIdAsync(eventId);

            if (eventEntity == null || eventEntity.EventUserId != currentUserId)
            {
                _logger.LogWarning("Unauthorized invite attempt by user {UserId} on event {EventId}.", currentUserId, eventId);
                throw new UnauthorizedAccessException("You do not have permission to invite users to this event");
            }

            var existingInvite = await _eventInviteRepository.GetInviteAsync(eventId, invitedUserId);
            if (existingInvite != null)
            {
                _logger.LogWarning("User {InvitedUserId} has already been invited to event {EventId}.",invitedUserId, eventId);
                throw new InvalidOperationException("This user has already been invited to this event");
            }

            var invite = new EventInvite
            {
                EventId = eventId,
                InvitedUserId = invitedUserId,
                Status = InviteStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Creating new invite for user {InvitedUserId} to event {EventId}.",
                invitedUserId, eventId);

            return await _eventInviteRepository.AddAsync(invite);
        }

        public async Task<EventInvite> RespondToInviteAsync(int inviteId, InviteStatus response, int userId)
        {
            _logger.LogInformation("User {UserId} is responding to invite {InviteId} with status {Response}.", userId, inviteId, response);

            var invite = await _eventInviteRepository.GetByIdAsync(inviteId);

            if (invite == null || invite.InvitedUserId != userId)
            {
                _logger.LogWarning("Unauthorized invite response attempt by user {UserId} on invite {InviteId}.", userId, inviteId);
                throw new UnauthorizedAccessException("You do not have permission to respond to this invitation");
            }

            if (invite.Status != InviteStatus.Pending)
            {
                throw new InvalidOperationException("This invitation has already been responded to");
            }

            if (response == InviteStatus.Accepted)
            {
                var hasConflict = await _eventRepository.HasTimeConflictAsync(
                    invite.Event.StartTime,
                    invite.Event.EndTime,
                    userId);

                if (hasConflict)
                {
                    throw new InvalidOperationException("There is a time conflict with another event");
                }
            }

            invite.Status = response;
            invite.ResponseAt = DateTime.UtcNow;
            await _eventInviteRepository.UpdateAsync(invite);

            _logger.LogInformation("User {UserId} successfully responded to invite {InviteId} with status {Response}.", userId, inviteId, response);

            return invite;
        }
    }
}
