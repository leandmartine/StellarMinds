using DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class ObtenerUsuarioCU : IObtenerUsuarios
{
    private IRepositorioUsuario repositorioUsuario;

    public ObtenerUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        repositorioUsuario = repositorioUsuario;
    }

    public List<UsuarioDTO> ObtenerUsuarios()
    {
        List<UsuarioDTO> aRetornar = new List<UsuarioDTO>();
        foreach (Usuario usuario in repositorioUsuario.FindAll())
        {
            aRetornar.Add(UsuarioDTOMapper.ToDto(usuario));
        }

        return aRetornar;
    }
}