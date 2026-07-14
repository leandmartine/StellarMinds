namespace StellarMindsMVC.Models;

public class ObservacionAltaModel
{
    public int PrestamoId { get; set; }
    public int ObjetoId { get; set; }
    public int SocioId { get; set; }
    public DateTime Fecha { get; set; }
    public TipoObservacion TipoObservacion { get; set; }

    // Resultado del botón "Evaluar" que se reenvía a la API para que la observación
    // se guarde con esa evaluación sin volver a consultar a Gemini.
    public string? Indicador { get; set; }
    public string? Detalle { get; set; }
}
