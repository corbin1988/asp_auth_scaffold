using Auth.Core.Modules.Auth.DTOs;
using Auth.Core.Modules.User;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Core.Modules.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto dto,
        [FromServices] IValidator<RegisterDto> validator,
        [FromServices] IUserService userService)
    {
        var validationResult = await validator.ValidateAsync(dto);

        if (ValidationHelper.HandleValidationResult(validationResult) is { } validationResponse)
            return validationResponse;

        await userService.CreateUserAsync(dto);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto,
        [FromServices] IValidator<LoginDto> validator,
        [FromServices] IUserService userService)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (ValidationHelper.HandleValidationResult(validationResult) is { } validationResponse)
            return validationResponse;

        var token = await userService.LoginAsync(dto);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { token });
    }
}