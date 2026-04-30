namespace StellarMinds.Entidades;

using StellarMinds.Entidades.Enums;

// Letra p.3: el socio solicita telescopio + montura obligatorios,
// y cámara u ocular (al menos uno, ambos opcionales por separado).
public class Prestamo
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoPrestamo Estado { get; set; }

    public Usuario Socio { get; set; }
    public Telescopio Telescopio { get; set; }
    public Montura Montura { get; set; }

    // Opcionales: debe haber al menos uno de los dos.
    public Camara? Camara { get; set; }
    public Ocular? Ocular { get; set; }
}