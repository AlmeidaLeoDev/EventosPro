using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class EventInviteValidator : AbstractValidator<EventInviteViewModel>
    {
        public EventInviteValidator() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid invite ID");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Created date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future");

            RuleFor(x => x.ResponseAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt).When(x => x.ResponseAt.HasValue)
                .WithMessage("Response date must be after the created date");

            RuleFor(x => x.EventId)
                .GreaterThan(0).When(x => x.EventId.HasValue)
                .WithMessage("Invalid event ID");

            RuleFor(x => x.InvitedUserId)
                .GreaterThan(0).When(x => x.InvitedUserId.HasValue)
                .WithMessage("Invalid invited user ID");

            RuleFor(x => x.InvitedUserName)
                .NotEmpty().WithMessage("Invited user name is required")
                .MaximumLength(100).WithMessage("Invited user name must not exceed 100 characters");

            RuleFor(x => x.InvitedUserEmail)
                .NotEmpty().WithMessage("Invited user email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters");
        }
    }
}
