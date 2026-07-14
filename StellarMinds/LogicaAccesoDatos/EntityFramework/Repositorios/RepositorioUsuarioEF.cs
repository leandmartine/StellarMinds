using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioUsuarioEF : IRepositorioUsuario
{
    private readonly StellarMindsContext _context;

    public RepositorioUsuarioEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Alta(Usuario aAgregar)
    {
        aAgregar.Validar();
        _context.Usuarios.Add(aAgregar);
        _context.SaveChanges();
    }

    public void Modificar(Usuario aModificar)
    {
        aModificar.Validar();
        Usuario existente = _context.Usuarios.FirstOrDefault(u => u.Id == aModificar.Id);
        if (existente == null)
            throw new UsuarioException("Usuario no encontrado");

        existente.NombreUsuario = aModificar.NombreUsuario;
        existente.Telefono = aModificar.Telefono;
        existente.Rol = aModificar.Rol;
        _context.SaveChanges();
    }

    public void Baja(int id)
    {
        Usuario existente = _context.Usuarios.FirstOrDefault(u => u.Id == id);
        if (existente == null)
            throw new UsuarioException("Usuario no encontrado");

        _context.Usuarios.Remove(existente);
        _context.SaveChanges();
    }

    public IEnumerable<Usuario> FindAll()
    {
        return _context.Usuarios.ToList();
    }

    public Usuario FindById(int id)
    {
        Usuario encontrado = _context.Usuarios.FirstOrDefault(u => u.Id == id);
        if (encontrado == null)
            throw new UsuarioException("Usuario no encontrado");
        return encontrado;
    }

    public void LoginUsuario(Usuario aLoguear)
    {
        Usuario usuario = _context.Usuarios
            .FirstOrDefault(u => u.NombreUsuario == aLoguear.NombreUsuario
                              && u.Contrasena.Pass == aLoguear.Contrasena.Pass);

        if (usuario == null)
            throw new UsuarioException("Usuario o contraseña incorrectos");
    }
}
