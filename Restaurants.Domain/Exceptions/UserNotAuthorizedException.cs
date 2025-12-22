namespace Restaurants.Domain.Exceptions;

public class UserNotAuthorizedException(string message="User not authorized"):Exception(message);