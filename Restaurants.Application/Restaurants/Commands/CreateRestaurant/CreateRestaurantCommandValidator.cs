using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> _validCategories = ["Italian", "Polish", "Japanese", "Spicy"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name).Length(3, 100).WithMessage("Name must be between 3 and 100 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Category)
            .Must(_validCategories.Contains)
            .WithMessage("Insert valid category");
        // Fully Implemented Validator Below
        //
        //     .Custom((category, context) =>
        // {
        //     var isValid = validCategories.Contains(category);
        //     if (!isValid) context.AddFailure("Invalid category");
        // });
        RuleFor(x => x.Category).NotEmpty().WithMessage("Insert valid category");
        RuleFor(x => x.ContactEmail).EmailAddress().WithMessage("Please provide a valid email address");
        RuleFor(x => x.PostalCode).Matches(@"^\d{2}-?\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX)");
    }
}