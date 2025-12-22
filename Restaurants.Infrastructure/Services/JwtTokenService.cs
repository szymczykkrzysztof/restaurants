using Microsoft.Extensions.Configuration;
using Restaurants.Application.Authorization;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService(
    IConfiguration config,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IJwtTokenService
{
    public async Task<string> GenerateTokenAsync(User user)
    {
        var jwt = config.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 🔹 Claims użytkownika
        var userClaims = await userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        // 🔹 Role użytkownika
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            
            var identityRole = await roleManager.FindByNameAsync(role);
            if (identityRole != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(identityRole);
                claims.AddRange(roleClaims);
            }
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMilliseconds(Convert.ToDouble(jwt["ExpirationMs"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
