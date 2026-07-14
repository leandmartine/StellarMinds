using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Equipos;

public interface IObtenerEquipoPorId
{
    EquipoDTO BuscarEquipoPorId(int id);
}
