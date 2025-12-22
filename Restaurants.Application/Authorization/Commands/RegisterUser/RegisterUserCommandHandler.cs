using MediatR;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Services;


namespace Restaurants.Application.Authorization.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    UserManager<User> userManager,
    IJwtTokenService jwt):IRequestHandler<RegisterUserCommand,string>
{
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
            throw new PasswordMismatchException();

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new UserAlreadyExistException();

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new PasswordMismatchException();
        var adminExists = (await userManager.GetUsersInRoleAsync("Admin")).Any();
        if (!adminExists)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            await userManager.AddToRoleAsync(user, "User");
        }

        // 🔹 (opcjonalnie) domyślny claim
        await userManager.AddClaimAsync(user,
            new System.Security.Claims.Claim("permission", "restaurants.read"));
        
        var token = await jwt.GenerateTokenAsync(user);
        return token;
    }
}