namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

// RF05: procesa la devolución de un préstamo.
// coordinadorId viene de la sesión y se usa para RF06 (auditoría automática).
public interface IDevolucionPrestamo
{
    void Devolver(int prestamoId, int coordinadorId);
}
