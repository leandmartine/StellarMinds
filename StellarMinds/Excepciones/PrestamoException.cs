namespace StellarMinds.Excepciones;

public class PrestamoException : Exception
{
    public PrestamoException() { }

    public PrestamoException(string message) : base(message) { }

    public PrestamoException(string? message, Exception? innerException) : base(message, innerException) { }
}