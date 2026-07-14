namespace StellarMinds.Entidades;

using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesDominio;

public class Prestamo : IValidable
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoPrestamo Estado { get; set; }
    public Usuario Socio { get; set; }
    public Montura Montura { get; set; }
    public Telescopio Telescopio { get; set; }
    public Camara? Camara { get; set; }
    public Ocular? Ocular { get; set; }

    public void Validar()
    {
        if (Socio == null)
            throw new PrestamoException("El socio es requerido");
        if (Telescopio == null)
            throw new PrestamoException("El telescopio es requerido");
        if (Montura == null)
            throw new PrestamoException("La montura es requerida");
        if (FechaFin <= FechaInicio)
            throw new PrestamoException("La fecha de fin debe ser posterior a la fecha de inicio");
        if (Camara == null && Ocular == null)
            throw new PrestamoException("Se debe incluir al menos una cámara o un ocular");
        if (Montura.CargaSoportada < Telescopio.Peso)
            throw new PrestamoException("La carga soportada por la montura es insuficiente para el telescopio");
        if (Camara != null &&
            Montura.Tipo != TipoMontura.ECUATORIAL &&
            Montura.Tipo != TipoMontura.HIBRIDA)
            throw new PrestamoException("Para usar cámara se requiere montura ecuatorial o híbrida");
        if (Telescopio.Stock <= 0)
            throw new PrestamoException("El telescopio no tiene stock disponible");
        if (Montura.Stock <= 0)
            throw new PrestamoException("La montura no tiene stock disponible");
        if (Camara != null && Camara.Stock <= 0)
            throw new PrestamoException("La cámara no tiene stock disponible");
        if (Ocular != null && Ocular.Stock <= 0)
            throw new PrestamoException("El ocular no tiene stock disponible");
    }
}
