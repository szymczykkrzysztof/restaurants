using MediatR;
using Restaurants.Application.Users.Commands.Dtos;

namespace Restaurants.Application.Users.Commands.LoginUser;

public class LoginUserCommand:IRequest<AuthResponseDto>
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}