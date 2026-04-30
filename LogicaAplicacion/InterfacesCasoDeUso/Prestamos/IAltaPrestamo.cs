using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

// RF04: crea un préstamo aplicando todas las reglas de negocio.
// coordinadorId viene de la sesión y se usa también para RF06 (auditoría).
public interface IAltaPrestamo
{
    void Alta(PrestamoDTO dto, int coordinadorId);
}
