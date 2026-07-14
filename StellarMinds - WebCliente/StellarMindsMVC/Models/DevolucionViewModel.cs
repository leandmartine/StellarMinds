using Microsoft.AspNetCore.Mvc.Rendering;

namespace StellarMindsMVC.Models;

public class DevolucionViewModel
{
    public int SocioId { get; set; }
    public IEnumerable<SelectListItem> Socios { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<PrestamoModel> PrestamosActivos { get; set; } = Enumerable.Empty<PrestamoModel>();
    public bool Buscado { get; set; }
}
