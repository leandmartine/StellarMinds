using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioUsuarioEF : IRepositorioUsuario
{
    private StellarMindsContext context;
    public RepositorioUsuarioEF()
    {
        context = new StellarMindsContext();
    }
    public void AltaUsuario(Usuario aAgregar)
    {
        aAgregar.Validar();
        context.Usuarios.Add(aAgregar);
        context.SaveChanges();
    }

    public void ActualizarUsuario(Usuario aModificar)
    {
        throw new NotImplementedException();
    }

    public void BajaUsuario(int aEliminar)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Usuario> FindAll()
    {
        return context.Usuarios;
    }

    public Usuario FindById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Usuario> FilterByNombreCompleto(string nombreCompleto)
    {
        throw new NotImplementedException();
    }
}