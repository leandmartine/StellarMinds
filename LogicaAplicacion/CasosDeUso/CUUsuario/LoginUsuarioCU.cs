using DTOs;
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

    public UsuarioDTO LoginUsuario(UsuarioDTO aLoguear)
    {
        // 1) Se valida credenciales contra el repositorio (si no matchea, tira UsuarioException).
        Usuario entrada = UsuarioDTOMapper.FromDto(aLoguear);
        repositorioUsuario.LoginUsuario(entrada);

        // 2) Se recupera el usuario completo (con Id y Rol) para devolverlo al controller.
        //    LINQ basico sobre FindAll: buscamos por NombreUsuario.
        Usuario logueado = repositorioUsuario
            .FindAll()
            .FirstOrDefault(u => u.NombreUsuario == entrada.NombreUsuario);

        if (logueado == null)
            throw new UsuarioException("No se pudo recuperar el usuario logueado");

        return UsuarioDTOMapper.ToDto(logueado);
    }
}
