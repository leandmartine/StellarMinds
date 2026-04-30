using StellarMinds.Entidades;
using StellarMinds.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.RepositorioMemoria;

public class RepositorioUsuario : IRepositorioUsuario
{
    // Lista estatica para mantener los usuarios entre requests mientras
    // el repositorio este registrado como Scoped.
    public static List<Usuario> Usuarios = new List<Usuario>();

    public RepositorioUsuario()
    {
        // Precarga: se corre una sola vez cuando la lista esta vacia.
        this.GenerarUsuarios();
    }

    public void Alta(Usuario aAgregar)
    {
        aAgregar.Validar();
        this.VerificarUsuarioExistente(aAgregar);

        // Asignamos Id autoincremental (lo exige la letra: Id secuencial otorgado por la BD).
        aAgregar.Id = Usuarios.Count == 0 ? 1 : Usuarios.Max(u => u.Id) + 1;
        Usuarios.Add(aAgregar);
    }

    public void LoginUsuario(Usuario aLoguear)
    {
        // LINQ basico: busca el primero que matchee usuario + contrasenha.
        bool encontrado = Usuarios.Any(u =>
            u.NombreUsuario == aLoguear.NombreUsuario &&
            u.Contrasena.Pass == aLoguear.Contrasena.Pass);

        if (!encontrado)
            throw new UsuarioException("Usuario o contrasena incorrectos");
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
        return Usuarios;
    }

    public Usuario FindById(int id)
    {
        // LINQ basico: FirstOrDefault devuelve null si no encuentra.
        Usuario encontrado = Usuarios.FirstOrDefault(u => u.Id == id);
        if (encontrado == null)
            throw new UsuarioException("Usuario no encontrado");
        return encontrado;
    }

    public void VerificarUsuarioExistente(Usuario aAgregar)
    {
        // Chequea por NombreUsuario (que es el identificador logico de login).
        bool existe = Usuarios.Any(u => u.NombreUsuario == aAgregar.NombreUsuario);
        if (existe)
            throw new UsuarioException("Ya existe un usuario con ese nombre de usuario");
    }

    public void GenerarUsuarios()
    {
        if (Usuarios.Count != 0) return;

        List<Usuario> usuariosPrecargados = new List<Usuario>
        {
            new Usuario("Juan Perez",       "Av. Italia",        1234, 0, "Av. San Martin",    "Montevideo", "Uruguay", "099123456", "juan.perez@gmail.com",       "jperez",      "Pass@1234", RolDeUsuario.SOCIO),
            new Usuario("Maria Garcia",     "Bulevar Artigas",    567, 0, "Francisco Vidal",   "Montevideo", "Uruguay", "098234567", "maria.garcia@gmail.com",     "mgarcia",     "Pass@5678", RolDeUsuario.SOCIO),
            new Usuario("Carlos Lopez",     "Av. 18 de Julio",    890, 0, "Yaguaron",          "Montevideo", "Uruguay", "097345678", "carlos.lopez@gmail.com",     "clopez",      "Pass@9012", RolDeUsuario.COORDINADOR),
            new Usuario("Ana Martinez",     "Colonia",            432, 0, "Convencion",        "Montevideo", "Uruguay", "096456789", "ana.martinez@gmail.com",     "amartinez",   "Pass@3456", RolDeUsuario.SOCIO),
            new Usuario("Luis Rodriguez",   "Rivera",             321, 0, "Larranaga",         "Montevideo", "Uruguay", "095567890", "luis.rodriguez@gmail.com",   "lrodriguez",  "Pass@7890", RolDeUsuario.COORDINADOR),
            new Usuario("Sofia Fernandez",  "Av. Libertador",     654, 0, "Av. Italia",        "Montevideo", "Uruguay", "094678901", "sofia.fernandez@gmail.com",  "sfernandez",  "Pass@2345", RolDeUsuario.SOCIO),
            new Usuario("Diego Gonzalez",   "Ejido",              987, 0, "San Jose",          "Montevideo", "Uruguay", "093789012", "diego.gonzalez@gmail.com",   "dgonzalez",   "Pass@6789", RolDeUsuario.ADMINISTRADOR),
            new Usuario("Laura Sanchez",    "Constitucion",       147, 0, "Democracia",        "Montevideo", "Uruguay", "092890123", "laura.sanchez@gmail.com",    "lsanchez",    "Pass@0123", RolDeUsuario.SOCIO),
            new Usuario("Martin Torres",    "Av. Brasil",         258, 0, "Reyes",             "Montevideo", "Uruguay", "091901234", "martin.torres@gmail.com",    "mtorres",     "Pass@4567", RolDeUsuario.COORDINADOR),
            new Usuario("Valentina Diaz",   "Canelones",          369, 0, "Magallanes",        "Montevideo", "Uruguay", "090012345", "valentina.diaz@gmail.com",   "vdiaz",       "Pass@8901", RolDeUsuario.SOCIO),
        };

        foreach (Usuario usuario in usuariosPrecargados)
        {
            // Usamos el Alta para que pase por validaciones + Id autoincremental.
            this.Alta(usuario);
        }
    }
}
