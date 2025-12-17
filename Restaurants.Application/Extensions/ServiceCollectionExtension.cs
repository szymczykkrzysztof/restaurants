using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddAutoMapper(_ => { }, applicationAssembly);
        services.AddValidatorsFromAssemblyContaining<CreateRestaurantDtoValidator>()
            .AddFluentValidationAutoValidation();
    }
}