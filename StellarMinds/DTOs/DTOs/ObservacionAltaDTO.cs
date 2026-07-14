using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class ObservacionAltaDTO
{
    public int PrestamoId { get; set; }
    public int ObjetoId { get; set; }
    public int SocioId { get; set; }
    public DateTime Fecha { get; set; }
    public TipoObservacion TipoObservacion { get; set; }

    // Resultado ya generado por el botón "Evaluar" del front (RF07).
    // Si vienen poblados, AltaObservacion los reutiliza y NO vuelve a llamar a Gemini.
    // Si vienen nulos, el caso de uso consulta a Gemini.
    public string? Indicador { get; set; }
    public string? Detalle { get; set; }
}
