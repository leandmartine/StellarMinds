using DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class AltaUsuarioCU : IAltaUsuario
{
    private readonly IRepositorioUsuario _repositorioUsuario;

    public AltaUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        _repositorioUsuario = repositorioUsuario;
    }

    public void AltaUsuario(UsuarioDTO aAgregar)
    {
        Usuario usuario = UsuarioDTOMapper.FromDto(aAgregar);
        _repositorioUsuario.Alta(usuario);
    }
}
