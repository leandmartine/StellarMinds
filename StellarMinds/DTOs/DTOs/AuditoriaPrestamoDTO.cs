using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class AuditoriaPrestamoDTO
{
    public int Id { get; set; }
    public AccionAuditoria Accion { get; set; }
    public DateTime Fecha { get; set; }
    public int CoordinadorId { get; set; }
    public string CoordinadorNombre { get; set; }
    public int PrestamoId { get; set; }
}
