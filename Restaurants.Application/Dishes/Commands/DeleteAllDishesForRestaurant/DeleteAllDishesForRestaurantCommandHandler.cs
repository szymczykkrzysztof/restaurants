using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantCommandHandler(
    ILogger<DeleteAllDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteAllDishesForRestaurantCommand>
{
    public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all dishes for restaurant : {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);
        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        await dishesRepository.Delete(restaurant.Dishes);
    }
}