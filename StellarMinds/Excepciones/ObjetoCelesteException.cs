namespace StellarMinds.Excepciones;

public class ObjetoCelesteException : Exception
{
    public ObjetoCelesteException() { }

    public ObjetoCelesteException(string message) : base(message) { }

    public ObjetoCelesteException(string? message, Exception? innerException) : base(message, innerException) { }

}