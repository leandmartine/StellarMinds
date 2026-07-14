using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

// DTO de LECTURA/salida de equipos (lo devuelven GetAll, GetById y el mapper ToDto).
// El alta y la modificación ya NO usan esta clase: cada subtipo tiene su propio DTO
// (TelescopioAltaDTO, MonturaAltaDTO, CamaraAltaDTO, OcularAltaDTO).
// Por eso acá no hay data annotations: las validaciones de entrada viven en los DTOs de alta.
// TipoEquipo actúa como discriminador para que el cliente sepa qué subtipo es cada equipo;
// los campos de un subtipo quedan en null cuando el equipo no es de ese tipo.
public class EquipoDTO
{
    public int Id { get; set; }

    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Stock { get; set; }

    // Discriminador: "TELESCOPIO", "MONTURA", "CAMARA", "OCULAR".
    public string TipoEquipo { get; set; }

    // --- Campos propios de Telescopio ---
    public decimal? Apertura { get; set; }
    public string RelFocal { get; set; }
    public decimal? DistanciaFocal { get; set; }
    public decimal? Peso { get; set; }

    // --- Campos propios de Montura ---
    public TipoMontura? TipoMontura { get; set; }
    public decimal? CargaSoportada { get; set; }
    public bool? Computorizado { get; set; }

    // --- Campos propios de Camara ---
    public string Sensor { get; set; }
    public string Resolucion { get; set; }
    public decimal? Pixel { get; set; }

    // --- Campos propios de Ocular ---
    public decimal? Diametro { get; set; }
    public decimal? Angulo { get; set; }
}
