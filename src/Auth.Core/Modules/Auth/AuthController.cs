using Auth.Core.Modules.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Core.Modules.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        // Simulate registration logic
        var result = new { Success = true, Message = "User registered successfully" };

        return Ok(new { message = result.Message });
    }
}