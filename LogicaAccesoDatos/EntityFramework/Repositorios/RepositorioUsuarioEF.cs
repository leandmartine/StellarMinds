using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioUsuarioEF : IRepositorioUsuario
{
    private StellarMindsContext context;

    public RepositorioUsuarioEF()
    {
        context = new StellarMindsContext();
    }

    public void Alta(Usuario aAgregar)
    {
        aAgregar.Validar();
        context.Usuarios.Add(aAgregar);
        context.SaveChanges();
    }

    public void Modificar(Usuario aModificar)
    {
        throw new NotImplementedException();
    }

    public void Baja(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Usuario> FindAll()
    {
        return context.Usuarios.ToList();
    }

    public Usuario FindById(int id)
    {
        Usuario encontrado = context.Usuarios.FirstOrDefault(u => u.Id == id);
        if (encontrado == null)
            throw new UsuarioException("Usuario no encontrado");
        return encontrado;
    }

    public void LoginUsuario(Usuario aLoguear)
    {
        var usuario = context.Usuarios
            .FirstOrDefault(u => u.NombreUsuario == aLoguear.NombreUsuario
                              && u.Contrasena.contrasenha == aLoguear.Contrasena.contrasenha);

        if (usuario == null)
            throw new UsuarioException("Usuario o contrasena incorrectos");
    }
}
