using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class CreateEventInviteValidator : AbstractValidator<CreateEventInviteViewModel>
    {
        public CreateEventInviteValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Invalid event ID");

            RuleFor(x => x.InvitedUserEmail)
                .NotEmpty().WithMessage("Invite email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters");
        }
    }
}
