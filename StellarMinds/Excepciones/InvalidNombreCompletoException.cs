namespace StellarMinds.Excepciones;

public class InvalidNombreCompletoException : Exception
{
    public InvalidNombreCompletoException(){ }
    
    public InvalidNombreCompletoException(string message) : base( message ) { }

    public InvalidNombreCompletoException(string? message, Exception? innerException) : base( message, innerException ) { }
}