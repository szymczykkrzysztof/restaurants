namespace Restaurants.Domain.Exceptions;

public class PasswordMismatchException(string message = "Password mismatch") : Exception(message);
