namespace Restaurants.API.Dtos;

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);

public record AuthResponseDto(
    string TokenType,
    string AccessToken,
    int ExpiresIn
);