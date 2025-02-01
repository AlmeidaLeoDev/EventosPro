using EventosPro.ViewModels.Users;
using FluentValidation;

namespace EventosPro.Validators.UserViewModelsValidators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");
        }
    }
}
