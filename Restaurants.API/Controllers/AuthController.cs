using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Authorization.Commands.LoginUser;
using Restaurants.Application.Authorization.Commands.RegisterUser;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        await mediator.Send(request);
        return Created();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        var response = await mediator.Send(request);

        return Ok(response);
    }
}