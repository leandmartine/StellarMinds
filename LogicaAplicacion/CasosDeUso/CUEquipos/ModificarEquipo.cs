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

    public void Modificar(EquipoDTO aModificar)
    {
        Equipo equipo = EquipoDTOMapper.FromDto(aModificar);
        _repositorioEquipo.Modificar(equipo);
    }
}
