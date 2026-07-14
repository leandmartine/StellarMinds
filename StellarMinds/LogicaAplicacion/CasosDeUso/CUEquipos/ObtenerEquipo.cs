using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class ObtenerEquipo : IObtenerEquipos
{
    private readonly IRepositorioEquipo _repositorioEquipo;

    public ObtenerEquipo(IRepositorioEquipo repositorioEquipo)
    {
        _repositorioEquipo = repositorioEquipo;
    }

    public List<EquipoDTO> ObtenerTodos()
    {
        List<EquipoDTO> resultado = new List<EquipoDTO>();
        foreach (Equipo e in _repositorioEquipo.FindAll())
            resultado.Add(EquipoDTOMapper.ToDto(e));
        return resultado;
    }
}
