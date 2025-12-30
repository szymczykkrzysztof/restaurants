using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<(List<Restaurant>, int )> GetAllMatchingAsync(string? searchPhrase,
        int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = dbContext.Restaurants
            .Where(r => searchPhraseLower == null || r.Name.ToLower()
                .Contains(searchPhraseLower) || r.Description.ToLower().Contains(searchPhraseLower));
        var totalCount = await baseQuery.CountAsync();
        if (sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category }
            };
            var selectedColumn = columnSelector[sortBy];
            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetById(int id)
    {
        return await dbContext
            .Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> Create(Restaurant entity)
    {
        await dbContext.Restaurants.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(Restaurant entity)
    {
        dbContext.Restaurants.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}