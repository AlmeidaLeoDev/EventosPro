using EventosPro.ViewModels.Users;
using FluentValidation;

namespace EventosPro.Validators.UserViewModelsValidators
{
    public class EmailConfirmationValidator : AbstractValidator<EmailConfirmationViewModel>
    {
        public EmailConfirmationValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
}
