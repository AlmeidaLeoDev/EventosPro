using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class EventListValidator : AbstractValidator<EventListViewModel>
    {
        public EventListValidator()
        {
            RuleFor(x => x.Events)
                .NotNull().WithMessage("Events list cannot be null");

            RuleFor(x => x.PendingInvites)
                .NotNull().WithMessage("Pending invites list cannot be null");
        }
    }
}
