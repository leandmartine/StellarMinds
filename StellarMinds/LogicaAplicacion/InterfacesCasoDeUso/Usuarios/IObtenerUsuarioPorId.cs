using DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface IObtenerUsuarioPorId
{
    UsuarioDTO BuscarUsuarioPorId(int id);
}
