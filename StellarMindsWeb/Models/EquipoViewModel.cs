using System.ComponentModel.DataAnnotations;
using StellarMinds.Entidades.Enums;

namespace StellarMindsWeb.Models;

// ViewModel unico para Alta y Edicion de equipos.
// Maneja los campos comunes + todos los campos especificos de las 4 variantes.
// El TipoEquipo (string) actua como discriminador que mapea al EquipoDTO.
public class EquipoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el tipo de equipo")]
    [Display(Name = "Tipo de equipo")]
    public string TipoEquipo { get; set; }

    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(50)]
    [Display(Name = "Marca")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "El modelo es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Modelo")]
    public string Modelo { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    // --- Telescopio ---
    [Range(0, double.MaxValue, ErrorMessage = "La apertura no puede ser negativa")]
    [Display(Name = "Apertura (mm)")]
    public decimal? Apertura { get; set; }

    [Display(Name = "Relacion focal (f/x)")]
    public string RelFocal { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "La distancia focal no puede ser negativa")]
    [Display(Name = "Distancia focal (mm)")]
    public decimal? DistanciaFocal { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El peso no puede ser negativo")]
    [Display(Name = "Peso (kg)")]
    public decimal? Peso { get; set; }

    // --- Montura ---
    [Display(Name = "Tipo de montura")]
    public TipoMontura? TipoMontura { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "La carga soportada no puede ser negativa")]
    [Display(Name = "Carga soportada (kg)")]
    public decimal? CargaSoportada { get; set; }

    [Display(Name = "Computorizada (GoTo)")]
    public bool Computorizado { get; set; }

    // --- Camara ---
    [Display(Name = "Tipo de sensor (CMOS/CCD)")]
    public string Sensor { get; set; }

    [Display(Name = "Resolucion (ej: 3840x2160)")]
    public string Resolucion { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El tamano del pixel no puede ser negativo")]
    [Display(Name = "Tamano del pixel (um)")]
    public decimal? Pixel { get; set; }

    // --- Ocular ---
    [Range(0, double.MaxValue, ErrorMessage = "El diametro no puede ser negativo")]
    [Display(Name = "Diametro (mm)")]
    public decimal? Diametro { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El angulo de vision no puede ser negativo")]
    [Display(Name = "Angulo de vision (grados)")]
    public decimal? Angulo { get; set; }
}
