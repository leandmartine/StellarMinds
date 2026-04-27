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
        // Se valida que el usuario exista en el dominio al momento del logout.
        // Si no existe, lanzamos excepcion para que el controller lo maneje.
        Usuario usuario = _repositorioUsuario.FindById(idUsuario);
        if (usuario == null)
            throw new UsuarioException("No se puede cerrar sesion: el usuario no existe");

        // Aca no se hace nada mas: la limpieza de la session cookie es
        // responsabilidad del controller (Session.Clear()), porque es un
        // detalle de presentacion/HTTP y no del dominio.
    }
}
