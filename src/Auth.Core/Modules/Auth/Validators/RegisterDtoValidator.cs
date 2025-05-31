using Auth.Core.Modules.Auth.DTOs;
using FluentValidation;

namespace Auth.Core.Modules.Auth.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");
    }
}