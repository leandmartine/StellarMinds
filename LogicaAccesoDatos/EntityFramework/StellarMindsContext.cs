using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;

namespace LogicaAccesoDatos.EntityFramework;

[Owned]
public class StellarMindsContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"SERVER=(localdb)\MsSqlLocalDB;Database=StellarMinds;integrated Security=True;");
    }
}