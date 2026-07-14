using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Observaciones;

public interface IAltaObservacion
{
    ObservacionDTO Alta(ObservacionAltaDTO dto);
}
