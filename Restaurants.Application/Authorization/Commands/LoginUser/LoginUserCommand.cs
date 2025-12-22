using MediatR;
using Restaurants.Application.Authorization.Dtos;

namespace Restaurants.Application.Authorization.Commands.LoginUser;

public class LoginUserCommand:IRequest<AuthResponseDto>
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}