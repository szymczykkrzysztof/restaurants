using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, IUserContext userContext)
    : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement", currentUser.Email,
            currentUser.DateOfBirth);
        if (currentUser.DateOfBirth == null || string.IsNullOrEmpty(currentUser.DateOfBirth.ToString()) ||
            string.IsNullOrWhiteSpace(currentUser.DateOfBirth.ToString()))
        {
            logger.LogWarning("User: {Email} does not have a date of birth", currentUser.Email);
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <=
            DateOnly.FromDateTime(DateTime.Now))
        {
            logger.LogInformation("User: {Email} is authorized", currentUser.Email);
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}