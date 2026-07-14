using DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class ObtenerUsuarioCU : IObtenerUsuarios
{
    private IRepositorioUsuario _repositorioUsuario;

    public ObtenerUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        _repositorioUsuario = repositorioUsuario;
    }

    public List<UsuarioDTO> ObtenerUsuarios()
    {
        List<UsuarioDTO> aRetornar = new List<UsuarioDTO>();
        if (_repositorioUsuario.FindAll() != null)
        {
            {
                foreach (Usuario usuario in _repositorioUsuario.FindAll())
                {
                    aRetornar.Add(UsuarioDTOMapper.ToDto(usuario));
                }
            }
        }

        return aRetornar;
    }
}