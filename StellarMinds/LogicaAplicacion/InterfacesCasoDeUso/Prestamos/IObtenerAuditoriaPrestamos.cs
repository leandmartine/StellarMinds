using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

public interface IObtenerAuditoriaPrestamos
{
    IEnumerable<AuditoriaPrestamoDTO> ObtenerTodas();

    IEnumerable<AuditoriaPrestamoDTO> ObtenerPorCoordinador(int coordinadorId);
}
