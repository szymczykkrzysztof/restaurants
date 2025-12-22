using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Services;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(User user);
}
