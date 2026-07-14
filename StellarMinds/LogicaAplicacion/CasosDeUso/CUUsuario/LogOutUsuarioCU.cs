using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUUsuario;

public class LogOutUsuarioCU : ILogOutUsuario
{
    private readonly IRepositorioUsuario _repositorioUsuario;

    public LogOutUsuarioCU(IRepositorioUsuario repositorioUsuario)
    {
        _repositorioUsuario = repositorioUsuario;
    }

    public void LogOut(int idUsuario)
    {
        // La sesión se limpia en el controller; acá solo validamos que el usuario exista.
        Usuario usuario = _repositorioUsuario.FindById(idUsuario);
        if (usuario == null)
            throw new UsuarioException("No se puede cerrar sesion: el usuario no existe");
    }
}
