using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class ModificarEquipo : IModificarEquipo
{
    private readonly IRepositorioEquipo _repositorioEquipo;

    public ModificarEquipo(IRepositorioEquipo repositorioEquipo)
    {
        _repositorioEquipo = repositorioEquipo;
    }

    public void Modificar(int id, EquipoAltaDTO aModificar)
    {
        // El DTO de alta no trae Id (es solo datos); el Id a modificar viene de la ruta.
        Equipo equipo = EquipoDTOMapper.FromAltaDto(aModificar);
        equipo.Id = id;
        _repositorioEquipo.Modificar(equipo);
    }
}
