using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Authorization;
using Restaurants.Infrastructure.Services;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");

        services.AddDbContext<RestaurantsDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
        });
        services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RestaurantsDbContext>()
            .AddDefaultTokenProviders();
        //services.AddIdentityApiEndpoints<User>();
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}