using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;

namespace LogicaAccesoDatos.EntityFramework;

public class StellarMindsContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<ObjetoCeleste> ObjetosCelestes { get; set; }
    public DbSet<Observacion> Observaciones { get; set; }
    public DbSet<AuditoriaPrestamo> AuditoriasPrestamo { get; set; }

    public StellarMindsContext(DbContextOptions<StellarMindsContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Equipo se mapea como una sola tabla con discriminador (herencia TPH).
        modelBuilder.Entity<Telescopio>();
        modelBuilder.Entity<Montura>();
        modelBuilder.Entity<Camara>();
        modelBuilder.Entity<Ocular>();

        modelBuilder.Entity<Usuario>()
            .OwnsOne(u => u.NombreCompleto)
            .Property(nc => nc.NombreCompletoTexto)
            .HasColumnName("NombreCompleto_nombreCompleto");

        // NoAction en las FK para que SQL Server no rechace el modelo por múltiples rutas de borrado en cascada.
        modelBuilder.Entity<Prestamo>(b =>
        {
            b.HasOne(p => p.Socio)
             .WithMany()
             .HasForeignKey("SocioId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(p => p.Telescopio)
             .WithMany()
             .HasForeignKey("TelescopioId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(p => p.Montura)
             .WithMany()
             .HasForeignKey("MonturaId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(p => p.Camara)
             .WithMany()
             .HasForeignKey("CamaraId")
             .IsRequired(false)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(p => p.Ocular)
             .WithMany()
             .HasForeignKey("OcularId")
             .IsRequired(false)
             .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Observacion>(b =>
        {
            b.HasOne(o => o.Prestamo)
             .WithMany()
             .HasForeignKey("PrestamoId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(o => o.Objeto)
             .WithMany()
             .HasForeignKey("ObjetoId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(o => o.Socio)
             .WithMany()
             .HasForeignKey("SocioId")
             .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AuditoriaPrestamo>(b =>
        {
            b.HasOne(a => a.Coordinador)
             .WithMany()
             .HasForeignKey("CoordinadorId")
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(a => a.Prestamo)
             .WithMany()
             .HasForeignKey("PrestamoId")
             .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
