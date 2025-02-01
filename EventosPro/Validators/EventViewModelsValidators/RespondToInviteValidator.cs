using EventosPro.ViewModels.Events;
using FluentValidation;

namespace EventosPro.Validators.EventViewModelsValidators
{
    public class RespondToInviteValidator : AbstractValidator<RespondToInviteViewModel>
    {
        public RespondToInviteValidator()
        {
            RuleFor(x => x.InviteId)
                .GreaterThan(0).WithMessage("Invalid invite ID");

            RuleFor(x => x.Response)
                .NotEmpty().WithMessage("Response is required")
                .Must(x => x == "Accepted" || x == "Declined")
                .WithMessage("Response must be either 'Accepted' or 'Declined'");
        }
    }
}
