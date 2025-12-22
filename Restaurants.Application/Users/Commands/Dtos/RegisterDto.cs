namespace Restaurants.Application.Users.Commands.Dtos;

public record AuthResponseDto(
    string TokenType,
    string AccessToken,
    int ExpiresInMs
);