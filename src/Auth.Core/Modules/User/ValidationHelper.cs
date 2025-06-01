using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Core.Modules.User;

public static class ValidationHelper
{
    public static IActionResult HandleValidationResult(ValidationResult validationResult)
    {
        if (validationResult.IsValid) return null!;

        return new BadRequestObjectResult(new
        {
            errors = validationResult.Errors.Select(e => new
            {
                field = e.PropertyName,
                message = e.ErrorMessage
            })
        });
    }
}