using System.ComponentModel.DataAnnotations;

namespace StellarMindsMVC.Models;

public class MisPrestamosViewModel
{
    [Required(ErrorMessage = "Ingrese el mes")]
    [Range(1, 12, ErrorMessage = "Mes inválido")]
    [Display(Name = "Mes")]
    public int Mes { get; set; } = DateTime.Today.Month;

    [Required(ErrorMessage = "Ingrese el año")]
    [Display(Name = "Año")]
    public int Anio { get; set; } = DateTime.Today.Year;

    public IEnumerable<PrestamoModel> Prestamos { get; set; } = Enumerable.Empty<PrestamoModel>();
    public bool Buscado { get; set; }
}
