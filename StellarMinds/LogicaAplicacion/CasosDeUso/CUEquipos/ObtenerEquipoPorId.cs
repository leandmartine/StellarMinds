using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class ObtenerEquipoPorId : IObtenerEquipoPorId
{
    private readonly IRepositorioEquipo _repositorioEquipo;

    public ObtenerEquipoPorId(IRepositorioEquipo repositorioEquipo)
    {
        _repositorioEquipo = repositorioEquipo;
    }

    public EquipoDTO BuscarEquipoPorId(int id)
    {
        Equipo equipo = _repositorioEquipo.FindById(id);
        return EquipoDTOMapper.ToDto(equipo);
    }
}
