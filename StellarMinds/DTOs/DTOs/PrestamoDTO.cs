using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class PrestamoDTO
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoPrestamo Estado { get; set; }
    public int SocioId { get; set; }
    public string SocioNombre { get; set; }
    public int TelescopioId { get; set; }
    public string TelescopioNombre { get; set; }
    public int MonturaId { get; set; }
    public string MonturaNombre { get; set; }
    public int? CamaraId { get; set; }
    public string? CamaraNombre { get; set; }
    public int? OcularId { get; set; }
    public string? OcularNombre { get; set; }
    public bool Atrasado { get; set; }

    // Datos técnicos que se le envían a Gemini para evaluar la observación (RF07).
    public decimal TelescopioApertura { get; set; }
    public decimal TelescopioFocal { get; set; }
    public string TelescopioRelFocal { get; set; }
    public string? CamaraSensor { get; set; }
    public string? CamaraResolucion { get; set; }
    public decimal? CamaraPixel { get; set; }
    public decimal? OcularDiametro { get; set; }
    public decimal? OcularCampo { get; set; }
}
