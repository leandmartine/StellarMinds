using StellarMinds.Entidades;

namespace StellarMinds.InterfacesRepositorios;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    public IEnumerable<Usuario> FilterByNombreCompleto(string nombreCompleto);
    
}