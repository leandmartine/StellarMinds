using DTOs;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class LoginUsuarioCU : ILoginUsuario
{
    private readonly IRepositorioUsuario repositorioUsuario;

    public LoginUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        this.repositorioUsuario = repositorioUsuario;
    }

    public UsuarioDTO LoginUsuario(UsuarioLoginDTO aLoguear)
    {
        Usuario encontrado = repositorioUsuario
            .FindAll()
            .FirstOrDefault(u => u.NombreUsuario == aLoguear.NombreUsuario
                              && u.Contrasena.Pass == aLoguear.Contrasena);

        if (encontrado == null)
            throw new UsuarioException("Usuario o contraseña incorrectos");

        return UsuarioDTOMapper.ToDto(encontrado);
    }
}
