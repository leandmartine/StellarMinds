using StellarMinds.InterfacesDominio;

namespace StellarMinds.Excepciones;

public class InvalidContrasenhaException : Exception
{
    public InvalidContrasenhaException(){ }
    
    public InvalidContrasenhaException(string message) : base( message ) { }

    public InvalidContrasenhaException(string? message, Exception? innerException) : base( message, innerException ) { }
}