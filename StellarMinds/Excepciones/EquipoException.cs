namespace StellarMinds.Excepciones;

public class EquipoException : Exception
{
    //ID del préstamo que bloquea la operación (null si no aplica).
    public int? PrestamoId { get; }

    //Estado del préstamo bloqueante: "EN PRÉSTAMO" o "ATRASADO" (null si no aplica)
    public string? TipoPrestamo { get; }

    public EquipoException() { }

    public EquipoException(string message) : base(message) { }

    public EquipoException(string? message, Exception? innerException) : base(message, innerException) { }

    //Constructor especial para bloqueo por préstamo activo (usado en BajaEquipo)
    public EquipoException(string message, int prestamoId, string tipoPrestamo) : base(message)
    {
        PrestamoId = prestamoId;
        TipoPrestamo = tipoPrestamo;
    }
}