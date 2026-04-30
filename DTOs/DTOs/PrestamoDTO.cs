using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

// DTO aplanado para Prestamo: usa IDs y strings descriptivos en lugar de entidades completas.
// Esto evita acoplar la capa de DTOs con el dominio (Clean Architecture).
public class PrestamoDTO
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoPrestamo Estado { get; set; }

    // Socio
    public int SocioId { get; set; }
    public string SocioNombre { get; set; }

    // Telescopio (obligatorio)
    public int TelescopioId { get; set; }
    public string TelescopioDescripcion { get; set; }

    // Montura (obligatorio)
    public int MonturaId { get; set; }
    public string MonturaDescripcion { get; set; }

    // Cámara (opcional — al menos una de las dos debe estar presente)
    public int? CamaraId { get; set; }
    public string? CamaraDescripcion { get; set; }

    // Ocular (opcional — al menos uno de los dos debe estar presente)
    public int? OcularId { get; set; }
    public string? OcularDescripcion { get; set; }
}
