namespace StellarMinds.Excepciones.ExcepcionesVO;

    public class InvalidDireccionException : Exception
    {
        public InvalidDireccionException(){ }
    
        public InvalidDireccionException(string message) : base( message ) { }

        public InvalidDireccionException(string? message, Exception? innerException) : base( message, innerException ) { }
    }