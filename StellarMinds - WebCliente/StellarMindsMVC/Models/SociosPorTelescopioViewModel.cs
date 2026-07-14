using Microsoft.AspNetCore.Mvc.Rendering;

namespace StellarMindsMVC.Models;

public class SociosPorTelescopioViewModel
{
    public int TelescopioId { get; set; }
    public IEnumerable<SelectListItem> Telescopios { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<UsuarioResumenModel> Socios { get; set; } = Enumerable.Empty<UsuarioResumenModel>();
    public bool Buscado { get; set; }
}
