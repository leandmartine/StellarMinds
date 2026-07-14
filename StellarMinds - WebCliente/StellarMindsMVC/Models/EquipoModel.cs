namespace StellarMindsMVC.Models;

public class EquipoModel
{
    public int Id { get; set; }
    public string TipoEquipo { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Stock { get; set; }

    // Telescopio
    public decimal? Apertura { get; set; }
    public string RelFocal { get; set; }
    public decimal? DistanciaFocal { get; set; }
    public decimal? Peso { get; set; }

    // Montura
    public TipoMontura? TipoMontura { get; set; }
    public decimal? CargaSoportada { get; set; }
    public bool? Computorizado { get; set; }

    // Camara
    public string Sensor { get; set; }
    public string Resolucion { get; set; }
    public decimal? Pixel { get; set; }

    // Ocular
    public decimal? Diametro { get; set; }
    public decimal? Angulo { get; set; }
}
