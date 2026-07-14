using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

public interface IObtenerPrestamosPorPeriodo
{
    IEnumerable<PrestamoDTO> ObtenerPorPeriodo(int socioId, int mes, int anio);
}
