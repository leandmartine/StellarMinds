using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

public interface IObtenerPrestamosPorSocio
{
    IEnumerable<PrestamoDTO> ObtenerActivosPorSocio(int socioId);
}
