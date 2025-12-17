namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    public string Name { get; set; } =string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}