namespace Restaurants.Infrastructure.Seeders;

public interface IRestaurantSeeder
{
    Task SeedAsync();
    Task SeedRolesAsync(IServiceProvider services);
}