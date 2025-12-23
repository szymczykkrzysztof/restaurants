using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Contracts;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail} to {Operation} for restaurant {RestaurantName}",
            user.Email, resourceOperation, restaurant.Name);
        switch (resourceOperation)
        {
            case ResourceOperation.Read:
            case ResourceOperation.Create:
                logger.LogInformation("Create/read operation - successful authorization");
                return true;
            case ResourceOperation.Delete when user.IsInRole(UserRoles.Admin):
                logger.LogInformation("Admin user, delete operation - successful authorization");
                return true;
            case ResourceOperation.Delete:
            case ResourceOperation.Update when user.Id == restaurant.OwnerId:
                logger.LogInformation("Restaurant owner - successful authorization");
                return true;
            default:
                return false;
        }
        
    }
}