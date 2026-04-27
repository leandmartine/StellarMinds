using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class AltaEquipo : IAltaEquipo
{
    private readonly IRepositorioEquipo _repositorioEquipo;

    public AltaEquipo(IRepositorioEquipo repositorioEquipo)
    {
        _repositorioEquipo = repositorioEquipo;
    }

    public void Alta(EquipoDTO aAgregar)
    {
        // Del DTO a entidad concreta (Telescopio, Montura, Camara u Ocular).
        Equipo equipo = EquipoDTOMapper.FromDto(aAgregar);

        // El repositorio hace la validacion de dominio + asigna Id autoincremental.
        _repositorioEquipo.Alta(equipo);
    }
}
