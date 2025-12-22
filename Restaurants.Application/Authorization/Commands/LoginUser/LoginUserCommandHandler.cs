using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Restaurants.Application.Authorization.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Services;

namespace Restaurants.Application.Authorization.Commands.LoginUser;

public class LoginUserCommandHandler(
    IConfiguration config,
    UserManager<User> userManager,
    IJwtTokenService jwt) : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var jwtConfigurationSection = config.GetSection("Jwt");
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new UserNotAuthorizedException();

        var valid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
            throw new UserNotAuthorizedException();

        var token = await jwt.GenerateTokenAsync(user);
        var authResponse = new AuthResponseDto("Bearer", token, Convert.ToInt32(jwtConfigurationSection["ExpirationMs"]));

        return authResponse;
    }
}