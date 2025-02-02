using EventosPro.Models;
using EventosPro.Services.Interfaces;
using EventosPro.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventosPro.Controllers
{
    /// <summary>
    /// Controller for managing event-related operations such as creating, updating, and deleting events.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, IUserService userService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _userService = userService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? throw new UnauthorizedAccessException("User not authenticated"));
        }

        /// <summary>
        /// Retrieves all events for the current user.
        /// </summary>
        /// <returns>A list of events.</returns>
        /// <response code="200">Events retrieved successfully.</response>
        /// <response code="500">If an error occurs during the retrieval process.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventListViewModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventListViewModel>> GetUserEvents()
        {
            try
            {
                var userId = GetCurrentUserId();
                var events = await _eventService.GetUserEventsAsync(userId);

                var viewModel = new EventListViewModel
                {
                    Events = events.Select(e => new EventDetailsViewModel
                    {
                        Id = e.Id,
                        Description = e.Description,
                        StartTime = e.StartTime,
                        EndTime = e.EndTime,
                        Status = e.Status.ToString(),
                        IsLongDuration = (e.EndTime - e.StartTime).TotalHours > 4,
                        CreatedAt = e.CreatedAt,
                        UpdatedAt = e.UpdatedAt,
                        EventUserId = e.EventUserId,
                        EventUserName = e.EventUser?.Name ?? "Unknown"
                    }).ToList()
                };

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events for user");
                return StatusCode(500, "An error occurred while retrieving events");
            }
        }

        /// <summary>
        /// Retrieves a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <returns>The event details.</returns>
        /// <response code="200">Event retrieved successfully.</response>
        /// <response code="404">If the event is not found.</response>
        /// <response code="500">If an error occurs during the retrieval process.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDetailsViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventDetailsViewModel>> GetEvent(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var eventEntity = await _eventService.GetEventByIdAsync(id, userId);

                if (eventEntity == null)
                {
                    return NotFound();
                }

                var viewModel = new EventDetailsViewModel
                {
                    Id = eventEntity.Id,
                    Description = eventEntity.Description,
                    StartTime = eventEntity.StartTime,
                    EndTime = eventEntity.EndTime,
                    Status = eventEntity.Status.ToString(),
                    IsLongDuration = (eventEntity.EndTime - eventEntity.StartTime).TotalHours > 4,
                    CreatedAt = eventEntity.CreatedAt,
                    UpdatedAt = eventEntity.UpdatedAt,
                    EventUserId = eventEntity.EventUserId,
                    EventUserName = eventEntity.EventUser?.Name ?? "Unknown"
                };

                return Ok(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching event {EventId}", id);
                return StatusCode(500, "An error occurred while retrieving the event");
            }
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="model">The event data.</param>
        /// <returns>The created event details.</returns>
        /// <response code="201">Event created successfully.</response>
        /// <response code="400">If the model state is invalid or the event data is invalid.</response>
        /// <response code="409">If there is a conflict with existing data.</response>
        /// <response code="500">If an error occurs during the creation process.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EventDetailsViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventDetailsViewModel>> CreateEvent([FromBody] CreateEventViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var eventEntity = new Event
                {
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime
                };

                var createdEvent = await _eventService.CreateEventAsync(eventEntity, userId);

                var viewModel = new EventDetailsViewModel
                {
                    Id = createdEvent.Id,
                    Description = createdEvent.Description,
                    StartTime = createdEvent.StartTime,
                    EndTime = createdEvent.EndTime,
                    Status = createdEvent.Status.ToString(),
                    CreatedAt = createdEvent.CreatedAt,
                    EventUserId = createdEvent.EventUserId
                };

                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, viewModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                return StatusCode(500, "An error occurred while creating the event");
            }
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="model">The updated event data.</param>
        /// <returns>The updated event details.</returns>
        /// <response code="200">Event updated successfully.</response>
        /// <response code="400">If the model state is invalid or the event data is invalid.</response>
        /// <response code="409">If there is a conflict with existing data.</response>
        /// <response code="500">If an error occurs during the update process.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDetailsViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventDetailsViewModel>> UpdateEvent(int id, [FromBody] UpdateEventViewModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var userId = GetCurrentUserId();

                var eventEntity = new Event
                {
                    Id = model.Id,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime
                };

                var updatedEvent = await _eventService.UpdateEventAsync(eventEntity, userId);

                var viewModel = new EventDetailsViewModel
                {
                    Id = updatedEvent.Id,
                    Description = updatedEvent.Description,
                    StartTime = updatedEvent.StartTime,
                    EndTime = updatedEvent.EndTime,
                    Status = updatedEvent.Status.ToString(),
                    UpdatedAt = updatedEvent.UpdatedAt,
                    EventUserId = updatedEvent.EventUserId
                };

                return Ok(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event {EventId}", id);
                return StatusCode(500, "An error occurred while updating the event");
            }
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <response code="204">Event deleted successfully.</response>
        /// <response code="500">If an error occurs during the deletion process.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _eventService.DeleteEventAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event {EventId}", id);
                return StatusCode(500, "An error occurred while deleting the event");
            }
        }

        /// <summary>
        /// Retrieves events within a specified date range.
        /// </summary>
        /// <param name="start">The start date of the range.</param>
        /// <param name="end">The end date of the range.</param>
        /// <returns>A list of events within the specified range.</returns>
        /// <response code="200">Events retrieved successfully.</response>
        /// <response code="500">If an error occurs during the retrieval process.</response>
        [HttpGet("range")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventDetailsViewModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EventDetailsViewModel>>> GetEventsInRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var userId = GetCurrentUserId();
                var events = await _eventService.GetEventsInRangeAsync(start, end, userId);

                var viewModels = events.Select(e => new EventDetailsViewModel
                {
                    Id = e.Id,
                    Description = e.Description,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Status = e.Status.ToString(),
                    IsLongDuration = (e.EndTime - e.StartTime).TotalHours > 4,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    EventUserId = e.EventUserId,
                    EventUserName = e.EventUser?.Name ?? "Unknown"
                });

                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events in range");
                return StatusCode(500, "An error occurred while retrieving events");
            }
        }

        /// <summary>
        /// Invites a user to an event.
        /// </summary>
        /// <param name="model">The invite data.</param>
        /// <returns>The created invite details.</returns>
        /// <response code="201">Invite created successfully.</response>
        /// <response code="404">If the invited user is not found.</response>
        /// <response code="409">If there is a conflict with existing data.</response>
        /// <response code="500">If an error occurs during the invite creation process.</response>
        [HttpPost("invite")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EventInviteViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventInviteViewModel>> CreateInvite([FromBody] CreateEventInviteViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();

                var invitedUser = await _userService.GetUserByEmailAsync(model.InvitedUserEmail);
                if (invitedUser == null)
                {
                    return NotFound("Invited user not found");
                }

                var invite = await _eventService.InviteUserToEventAsync(model.EventId, invitedUser.Id, userId);

                var viewModel = new EventInviteViewModel
                {
                    Id = invite.Id,
                    Status = invite.Status.ToString(),
                    CreatedAt = invite.CreatedAt,
                    EventId = invite.EventId,
                    InvitedUserId = invite.InvitedUserId,
                    InvitedUserEmail = invitedUser.Email,
                    InvitedUserName = invitedUser.Name
                };

                return CreatedAtAction(nameof(GetEvent), new { id = model.EventId }, viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invite");
                return StatusCode(500, "An error occurred while creating the invite");
            }
        }

        /// <summary>
        /// Responds to an event invite.
        /// </summary>
        /// <param name="model">The invite response data.</param>
        /// <returns>The updated invite details.</returns>
        /// <response code="200">Invite response processed successfully.</response>
        /// <response code="400">If the response data is invalid.</response>
        /// <response code="500">If an error occurs during the response processing.</response>
        [HttpPost("invite-respond")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventInviteViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventInviteViewModel>> RespondToInvite([FromBody] RespondToInviteViewModel model)
        {
            try
            {
                var userId = GetCurrentUserId();
                var inviteStatus = Enum.Parse<InviteStatus>(model.Response, true);

                var invite = await _eventService.RespondToInviteAsync(model.InviteId, inviteStatus, userId);

                var viewModel = new EventInviteViewModel
                {
                    Id = invite.Id,
                    Status = invite.Status.ToString(),
                    CreatedAt = invite.CreatedAt,
                    ResponseAt = invite.ResponseAt,
                    EventId = invite.EventId,
                    InvitedUserId = invite.InvitedUserId
                };

                return Ok(viewModel);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error responding to invite");
                return StatusCode(500, "An error occurred while responding to the invite");
            }
        }
    }
}
