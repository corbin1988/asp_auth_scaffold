using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.User;
using FluentValidation;

namespace Auth.Core.Modules.Auth.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellation) =>
            {
                var user = await userRepository.GetUserByEmailAsync(email);
                return user == null;
            })
            .WithMessage("A user with this email already exists.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");
    }
}