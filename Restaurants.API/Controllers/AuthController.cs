using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Dtos;
using Restaurants.API.Services;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<User> userManager,
    JwtTokenService jwt) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto request)
    {
        if (request.Password != request.ConfirmPassword)
            return BadRequest("Passwords do not match");

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return Conflict("User already exists");

        var user = new User
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);
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

        return Ok(new AuthResponseDto(
            TokenType: "Bearer",
            AccessToken: token,
            ExpiresIn: 3600
        ));
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Unauthorized();

        var valid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
            return Unauthorized();

        var token = await jwt.GenerateTokenAsync(user);

        return Ok(new
        {
            tokenType = "Bearer",
            accessToken = token,
            expiresIn = 3600
        });
    }
}
