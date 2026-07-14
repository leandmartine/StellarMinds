namespace StellarMindsMVC.Models;

public class AuditoriaViewModel
{
    public IEnumerable<AuditoriaModel> Auditorias { get; set; } = [];
    public IEnumerable<UsuarioResumenModel> Coordinadores { get; set; } = [];
    public int? CoordinadorIdFiltro { get; set; }
}
