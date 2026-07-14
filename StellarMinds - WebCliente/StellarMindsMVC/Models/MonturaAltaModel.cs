namespace StellarMindsMVC.Models;

public class MonturaAltaModel : EquipoAltaModel
{
    public TipoMontura TipoMontura { get; set; }
    public decimal CargaSoportada { get; set; }
    public bool Computorizado { get; set; }
}
