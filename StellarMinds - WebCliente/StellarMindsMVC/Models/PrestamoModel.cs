namespace StellarMindsMVC.Models;

public class PrestamoModel
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public EstadoPrestamo Estado { get; set; }
    public int SocioId { get; set; }
    public string SocioNombre { get; set; }
    public int TelescopioId { get; set; }
    public string TelescopioNombre { get; set; }
    public int MonturaId { get; set; }
    public string MonturaNombre { get; set; }
    public int? CamaraId { get; set; }
    public string? CamaraNombre { get; set; }
    public int? OcularId { get; set; }
    public string? OcularNombre { get; set; }
    public bool Atrasado { get; set; }
}
