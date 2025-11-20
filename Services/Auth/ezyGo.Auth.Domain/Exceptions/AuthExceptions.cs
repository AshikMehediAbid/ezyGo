namespace ezyGo.Auth.Domain.Exceptions;

public class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
    public AuthException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidCredentialsException : AuthException
{
    public InvalidCredentialsException() : base("Invalid email or password") { }
}

public class UserAlreadyExistsException : AuthException
{
    public UserAlreadyExistsException(string email) : base($"User with email '{email}' already exists") { }
}

public class UserNotFoundException : AuthException
{
    public UserNotFoundException(string email) : base($"User with email '{email}' not found") { }
}

public class ValidationException : AuthException
{
    public ValidationException(string message) : base(message) { }
}