using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Contracts;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(User user);
}
