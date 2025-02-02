using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class EventDetailsValidator : AbstractValidator<EventDetailsViewModel>
    {
        public EventDetailsValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid event ID");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start date/time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End date/time is required")
                .GreaterThan(x => x.StartTime).WithMessage("End date/time must be after the start date/time");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Created date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt).When(x => x.UpdatedAt.HasValue)
                .WithMessage("Updated date must be after the created date");

            RuleFor(x => x.EventUserId)
                .GreaterThan(0).WithMessage("Invalid event user ID");

            RuleFor(x => x.EventUserName)
                .NotEmpty().WithMessage("Event user name is required")
                .MaximumLength(100).WithMessage("Event user name must not exceed 100 characters");

            RuleFor(x => x.Invites)
                .NotNull().WithMessage("Invites list cannot be null");
        }
    }
}
