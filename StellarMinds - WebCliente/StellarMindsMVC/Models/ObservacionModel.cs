namespace StellarMindsMVC.Models;

// Espejo de ObservacionDTO de la API. Se usa para leer la respuesta del alta
// (incluida la Advertencia) y para listar las observaciones en Index.
public class ObservacionModel
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

    // Presente cuando la observación se guardó pero Gemini no pudo evaluarla.
    public string? Advertencia { get; set; }
}
