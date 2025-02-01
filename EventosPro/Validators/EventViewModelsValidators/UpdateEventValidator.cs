using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class UpdateEventValidator : AbstractValidator<UpdateEventViewModel>
    {
        public UpdateEventValidator()
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
        }
    }
}
 