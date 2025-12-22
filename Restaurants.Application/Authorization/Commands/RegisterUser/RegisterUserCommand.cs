using System.Security.AccessControl;
using MediatR;

namespace Restaurants.Application.Authorization.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<string>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}