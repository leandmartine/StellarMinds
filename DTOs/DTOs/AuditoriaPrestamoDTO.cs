using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

// DTO para mostrar la información de auditoría de un préstamo (RF06).
public class AuditoriaPrestamoDTO
{
    public int Id { get; set; }
    public AccionAuditoria Accion { get; set; }
    public DateTime Fecha { get; set; }
    public string CoordinadorNombre { get; set; }
    public int PrestamoId { get; set; }
}
