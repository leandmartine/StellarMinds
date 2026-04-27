using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Equipos;

public interface IObtenerEquipos
{
    List<EquipoDTO> ObtenerTodos();
}
