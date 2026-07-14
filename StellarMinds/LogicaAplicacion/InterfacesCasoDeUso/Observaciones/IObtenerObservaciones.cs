using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Observaciones;

public interface IObtenerObservaciones
{
    IEnumerable<ObservacionDTO> ObtenerTodas();
}
