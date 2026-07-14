using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StellarMindsMVC.Models;

public class PrestamoAltaViewModel
{
    [Required(ErrorMessage = "Seleccione un socio")]
    [Display(Name = "Socio")]
    public int SocioId { get; set; }

    [Required(ErrorMessage = "Seleccione un telescopio")]
    [Display(Name = "Telescopio")]
    public int TelescopioId { get; set; }

    [Required(ErrorMessage = "Seleccione una montura")]
    [Display(Name = "Montura")]
    public int MonturaId { get; set; }

    [Display(Name = "Cámara (opcional)")]
    public int? CamaraId { get; set; }

    [Display(Name = "Ocular (opcional)")]
    public int? OcularId { get; set; }

    [Required(ErrorMessage = "Ingrese la fecha de inicio")]
    [Display(Name = "Fecha de inicio")]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Ingrese la fecha de fin")]
    [Display(Name = "Fecha de fin")]
    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(7);

    public IEnumerable<SelectListItem> Socios { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Telescopios { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Monturas { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Camaras { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Oculares { get; set; } = Enumerable.Empty<SelectListItem>();
}
