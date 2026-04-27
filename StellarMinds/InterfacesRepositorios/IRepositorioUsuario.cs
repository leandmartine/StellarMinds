using StellarMinds.Entidades;

namespace StellarMinds.InterfacesRepositorios;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    // Metodo propio del repositorio de usuarios: no aplica a equipos ni a prestamos
    // por eso lo sacamos del contrato generico y lo declaramos aca.
    void LoginUsuario(Usuario aLoguear);
}
