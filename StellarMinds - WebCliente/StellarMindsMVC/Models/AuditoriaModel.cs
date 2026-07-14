namespace StellarMindsMVC.Models;

public enum AccionAuditoria
{
    ALTA_PRESTAMO,
    DEVOLUCION
}

public class AuditoriaModel
{
    public int Id { get; set; }
    public AccionAuditoria Accion { get; set; }
    public DateTime Fecha { get; set; }
    public int CoordinadorId { get; set; }
    public string CoordinadorNombre { get; set; }
    public int PrestamoId { get; set; }
}
