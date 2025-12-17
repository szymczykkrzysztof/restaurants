using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Modifying restaurant with id: {RequestId}", request.Id);
        var restaurant = await restaurantsRepository.GetById(request.Id);
        if (restaurant == null)
            return false;
        
        mapper.Map(request, restaurant);
        await restaurantsRepository.Update(restaurant);
        return true;
    }
}