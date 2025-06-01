public async Task<IActionResult> CreateUserAsync(RegisterDto dto)
{
    var validationResult = await validator.ValidateAsync(dto);
    if (ValidationHelper.HandleValidationResult(validationResult) is { } validationResponse)
        return validationResponse;

    var hashedPassword = HashPassword(dto.Password);

    var user = new UserEntity
    {
        Email = dto.Email,
        PasswordHash = hashedPassword
    };

    await userRepository.CreateUserAsync(user); // Adjusted to handle void return
    return new OkResult(); // Return success response
}
