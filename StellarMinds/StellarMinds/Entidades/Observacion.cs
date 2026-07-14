namespace StellarMinds.Entidades;

using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesDominio;

public class Observacion : IValidable
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public Prestamo Prestamo { get; set; }
    public ObjetoCeleste Objeto { get; set; }
    public Usuario Socio { get; set; }
    public IndicadorAdecuacion Indicador { get; set; }
    public string Detalle { get; set; }
    public TipoObservacion TipoObservacion { get; set; }

    public void Validar()
    {
        if (Socio == null)
            throw new ObservacionException("El socio es requerido");
        if (Prestamo == null)
            throw new ObservacionException("El préstamo es requerido");
        if (Objeto == null)
            throw new ObservacionException("El objeto celeste es requerido");
        if (Fecha == default)
            throw new ObservacionException("La fecha de observación es requerida");
        if (string.IsNullOrWhiteSpace(Detalle))
            throw new ObservacionException("El detalle de la observación es requerido");
    }
}
