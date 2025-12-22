namespace Restaurants.Application.Authorization.Dtos;

public record AuthResponseDto(
    string TokenType,
    string AccessToken,
    int ExpiresInMs
);