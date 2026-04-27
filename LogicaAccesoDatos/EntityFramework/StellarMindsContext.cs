using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;

namespace LogicaAccesoDatos.EntityFramework;

public class StellarMindsContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    // Un solo DbSet para la clase raiz de la jerarquia.
    // EF Core detecta automaticamente Telescopio, Montura, Camara y Ocular
    // porque heredan de Equipo, y genera una unica tabla "Equipos" con una
    // columna "Discriminator" que dice de que tipo es cada fila (TPH por defecto).
    public DbSet<Equipo> Equipos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"SERVER=(localdb)\MsSqlLocalDB;Database=StellarMinds;integrated Security=True;");
    }
}
