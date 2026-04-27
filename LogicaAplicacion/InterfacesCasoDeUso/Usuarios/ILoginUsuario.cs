using DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface ILoginUsuario
{
    // Devuelve el UsuarioDTO del logueado (con Id + Rol) para que el
    // controller lo pueda guardar en la variable de sesion.
    UsuarioDTO LoginUsuario(UsuarioDTO aLoguear);
}
