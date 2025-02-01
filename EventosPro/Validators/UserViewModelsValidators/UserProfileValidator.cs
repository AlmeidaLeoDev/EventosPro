using EventosPro.ViewModels.Users;
using FluentValidation;

namespace EventosPro.Validators.UserViewModelsValidators
{
    public class UserProfileValidator : AbstractValidator<UserProfileViewModel>
    {
        public UserProfileValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid user ID");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Created date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future");

            RuleFor(x => x.LastLoginAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Last login date cannot be in the future")
                .GreaterThanOrEqualTo(x => x.CreatedAt).When(x => x.LastLoginAt.HasValue)
                .WithMessage("Last login date must be after the created date");
        }
    }
}
