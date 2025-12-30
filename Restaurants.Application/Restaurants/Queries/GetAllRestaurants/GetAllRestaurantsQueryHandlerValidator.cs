using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandlerValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private string[] allowSortByColumnNames =
        [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description)];

    private int[] allowPageSizes = [5, 10, 15, 30];

    public GetAllRestaurantsQueryHandlerValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).Must(x => allowPageSizes.Contains(x))
            .WithMessage($"Page size must be in [{string.Join(", ", allowPageSizes)}]");
        RuleFor(r => r.SortBy)
            .Must(x => allowSortByColumnNames.Contains(x))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional or must be in [{string.Join(", ", allowSortByColumnNames)}]");
    }
}