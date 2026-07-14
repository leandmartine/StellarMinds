using DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class ObtenerUsuarioPorIdCU : IObtenerUsuarioPorId
{
    private readonly IRepositorioUsuario _repositorioUsuario;

    public ObtenerUsuarioPorIdCU(IRepositorioUsuario repositorioUsuario)
    {
        _repositorioUsuario = repositorioUsuario;
    }

    public UsuarioDTO BuscarUsuarioPorId(int id)
    {
        Usuario usuario = _repositorioUsuario.FindById(id);
        return UsuarioDTOMapper.ToDto(usuario);
    }
}
