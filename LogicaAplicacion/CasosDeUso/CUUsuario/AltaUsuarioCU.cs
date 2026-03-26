using DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class AltaUsuarioCU : IAltaUsuario
{
    private IRepositorioUsuario repositorioUsuario;

    public AltaUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        repositorioUsuario = repositorioUsuario;
    }

    public void AltaUsuario(UsuarioDTO aAgregar)
    {
        repositorioUsuario.AltaUsuario(UsuarioDTOMapper.FromDto(aAgregar));
    }
}