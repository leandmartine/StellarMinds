using DTOs;
using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Usuarios;

public interface ILoginUsuario
{
    UsuarioDTO LoginUsuario(UsuarioLoginDTO aLoguear);
}
