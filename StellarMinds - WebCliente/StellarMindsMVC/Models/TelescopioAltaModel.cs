namespace StellarMindsMVC.Models;

public class TelescopioAltaModel : EquipoAltaModel
{
    public decimal Apertura { get; set; }
    public string RelFocal { get; set; }
    public decimal DistanciaFocal { get; set; }
    public decimal Peso { get; set; }
}
