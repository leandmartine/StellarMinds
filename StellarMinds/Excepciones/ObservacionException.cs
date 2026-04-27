namespace StellarMinds.Excepciones;

public class ObservacionException : Exception
{
    public ObservacionException() { }

    public ObservacionException(string message) : base(message) { }

    public ObservacionException(string? message, Exception? innerException) : base(message, innerException) { }
}