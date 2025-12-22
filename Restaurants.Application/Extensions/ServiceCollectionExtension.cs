using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(_ => { }, applicationAssembly);
        services.AddValidatorsFromAssemblyContaining<CreateRestaurantCommandValidator>()
            .AddFluentValidationAutoValidation();
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
    }
}