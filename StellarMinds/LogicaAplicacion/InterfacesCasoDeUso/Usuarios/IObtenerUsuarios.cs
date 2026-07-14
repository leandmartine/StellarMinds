using DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface IObtenerUsuarios
{
    public List<UsuarioDTO> ObtenerUsuarios();
}