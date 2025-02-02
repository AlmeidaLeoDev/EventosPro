using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class CreateEventValidator : AbstractValidator<CreateEventViewModel>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start date/time is required")
                .GreaterThan(DateTime.Now).WithMessage("Start date/time must be in the future");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End date/time is required")
                .GreaterThan(x => x.StartTime).WithMessage("End date/time must be after the start date/time"); 
        }
    }
}
