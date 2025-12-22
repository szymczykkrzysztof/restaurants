namespace Restaurants.Domain.Exceptions;

public class UserAlreadyExistException(string message = "User already exist") : Exception(message);