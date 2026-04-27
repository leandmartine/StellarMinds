using DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface IObtenerUsuarioPorId
{
    // "BuscarUsuarioPorId" que vamos a usar desde el filtro de autorizacion
    // para saber el rol del usuario logueado (guardamos el Id en sesion).
    UsuarioDTO BuscarUsuarioPorId(int id);
}
