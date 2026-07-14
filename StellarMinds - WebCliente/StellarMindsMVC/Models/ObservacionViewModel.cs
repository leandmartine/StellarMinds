using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StellarMindsMVC.Models;

public class ObservacionViewModel
{
    [Required(ErrorMessage = "Seleccione un préstamo activo")]
    [Display(Name = "Préstamo activo")]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Seleccione un objeto celeste")]
    [Display(Name = "Objeto celeste")]
    public int ObjetoId { get; set; }

    [Required(ErrorMessage = "Seleccione el tipo de observación")]
    [Display(Name = "Tipo de observación")]
    public TipoObservacion TipoObservacion { get; set; }

    [Required(ErrorMessage = "Ingrese la fecha")]
    [Display(Name = "Fecha")]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    public string? IndicadorResultado { get; set; }
    public string? DetalleResultado { get; set; }

    public IEnumerable<SelectListItem> Prestamos { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> ObjetosCelestes { get; set; } = Enumerable.Empty<SelectListItem>();
}
