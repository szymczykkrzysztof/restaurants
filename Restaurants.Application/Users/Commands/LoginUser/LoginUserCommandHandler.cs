using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Restaurants.Application.Users.Commands.Dtos;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.LoginUser;

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