using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

// RF05: obtiene los préstamos activos (EN_PRESTAMO) de un socio para la devolución.
public interface IObtenerPrestamosPorUsuario
{
    IEnumerable<PrestamoDTO> ObtenerActivosPorUsuario(int socioId);
}
