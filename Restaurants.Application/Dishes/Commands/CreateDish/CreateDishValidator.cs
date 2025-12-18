using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishValidator()
    {
        RuleFor(d => d.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");
        RuleFor(d => d.KiloCalories).GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be greater than or equal to 0");
    }
}