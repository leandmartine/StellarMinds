using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class ObservacionDTO
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int PrestamoId { get; set; }
    public int ObjetoId { get; set; }
    public string ObjetoNombre { get; set; }
    public int SocioId { get; set; }
    public string SocioNombre { get; set; }
    public IndicadorAdecuacion Indicador { get; set; }
    public string Detalle { get; set; }
    public TipoObservacion TipoObservacion { get; set; }

    // Mensaje opcional cuando la observación se guardó pero Gemini no pudo evaluarla
    // (429, 400, sin conexión, etc.). El front lo muestra por ViewBag. Null si todo salió bien.
    public string? Advertencia { get; set; }
}
