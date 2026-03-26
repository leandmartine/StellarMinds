using StellarMinds.Entidades;
using StellarMinds.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.RepositorioMemoria;

public class RepositorioUsuario : IRepositorioUsuario
{
    public static List<Usuario> Usuarios = new List<Usuario>();

    public RepositorioUsuario()
    {
        this.GenerarUsuarios();
    }

    public void AltaUsuario(Usuario aAgregar)
    {
        try
        {
            aAgregar.Validar();
            this.VerificarUsuarioExistente(aAgregar);
            Usuarios.Add(aAgregar);
        }
        catch (UsuarioException ex)
        {
            
        }
    }

    public void ActualizarUsuario(Usuario aModificar)
    {
        throw new NotImplementedException();
    }

    public void BajaUsuario(int aEliminar)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Usuario> IRepositorio<Usuario>.FindAll()
    {
        return FindAll();
    }

    public Usuario FindById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Usuario> FilterByNombreCompleto(string nombreCompleto)
    {
        throw new NotImplementedException();
    }

    public List<Usuario> FindAll()
    {
        Usuarios.Clear();
        this.GenerarUsuarios();
        return Usuarios;
    }

    public void VerificarUsuarioExistente(Usuario aAgregar)
    {
        foreach (Usuario usuario in Usuarios)
        {
            if (usuario.Id == aAgregar.Id)
            {
                throw new UsuarioException("Usuario ya existe");
            }
        }
    }

    public void GenerarUsuarios()
    {
        if (Usuarios.Count != 0) return;

        List<Usuario> usuariosPrecargados = new List<Usuario>
        {
            new Usuario("Juan Pérez",       "Av. Italia",        1234, 0, "Av. San Martín",    "Montevideo", "Uruguay", "099123456", "juan.perez@gmail.com",       "jperez",      "Pass@1234", RolDeUsuario.SOCIO),
            new Usuario("María García",      "Bulevar Artigas",    567, 0, "Francisco Vidal",   "Montevideo", "Uruguay", "098234567", "maria.garcia@gmail.com",     "mgarcia",     "Pass@5678", RolDeUsuario.SOCIO),
            new Usuario("Carlos López",     "Av. 18 de Julio",    890, 0, "Yaguarón",          "Montevideo", "Uruguay", "097345678", "carlos.lopez@gmail.com",     "clopez",      "Pass@9012", RolDeUsuario.COORDINADOR),
            new Usuario("Ana Martínez",     "Colonia",            432, 0, "Convención",        "Montevideo", "Uruguay", "096456789", "ana.martinez@gmail.com",     "amartinez",   "Pass@3456", RolDeUsuario.SOCIO),
            new Usuario("Luis Rodríguez",   "Rivera",             321, 0, "Larrañaga",         "Montevideo", "Uruguay", "095567890", "luis.rodriguez@gmail.com",   "lrodriguez",  "Pass@7890", RolDeUsuario.COORDINADOR),
            new Usuario("Sofía Fernández",  "Av. Libertador",     654, 0, "Av. Italia",        "Montevideo", "Uruguay", "094678901", "sofia.fernandez@gmail.com",  "sfernandez",  "Pass@2345", RolDeUsuario.SOCIO),
            new Usuario("Diego González",   "Ejido",              987, 0, "San José",          "Montevideo", "Uruguay", "093789012", "diego.gonzalez@gmail.com",   "dgonzalez",   "Pass@6789", RolDeUsuario.ADMINISTRADOR),
            new Usuario("Laura Sánchez",    "Constitución",       147, 0, "Democracia",        "Montevideo", "Uruguay", "092890123", "laura.sanchez@gmail.com",    "lsanchez",    "Pass@0123", RolDeUsuario.SOCIO),
            new Usuario("Martín Torres",    "Av. Brasil",         258, 0, "Reyes",             "Montevideo", "Uruguay", "091901234", "martin.torres@gmail.com",    "mtorres",     "Pass@4567", RolDeUsuario.COORDINADOR),
            new Usuario("Valentina Díaz",   "Canelones",          369, 0, "Magallanes",        "Montevideo", "Uruguay", "090012345", "valentina.diaz@gmail.com",   "vdiaz",       "Pass@8901", RolDeUsuario.SOCIO),
        };

        int id = 1;
        foreach (Usuario usuario in usuariosPrecargados)
        {
            usuario.Id = id++;
            this.AltaUsuario(usuario);
        }
    }
}