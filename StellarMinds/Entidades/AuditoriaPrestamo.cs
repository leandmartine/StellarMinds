namespace StellarMinds.Entidades;

using StellarMinds.Entidades.Enums;

// RF06: cada alta y cada devolución de préstamo genera un registro de auditoría.
// Contiene qué se hizo (AccionAuditoria), cuándo, quién lo hizo y sobre qué préstamo.
public class AuditoriaPrestamo
{
    public int Id { get; set; }
    public AccionAuditoria Accion { get; set; }
    public DateTime Fecha { get; set; }
    public Usuario Coordinador { get; set; }
    public Prestamo Prestamo { get; set; }
}
