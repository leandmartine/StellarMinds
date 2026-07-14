using StellarMinds.Entidades.Enums;

namespace StellarMinds.Entidades;

public class AuditoriaPrestamo
{
    public int Id { get; set; }
    public AccionAuditoria Accion { get; set; }
    public DateTime Fecha { get; set; }
    public Usuario Coordinador { get; set; }
    public Prestamo Prestamo { get; set; }
}
