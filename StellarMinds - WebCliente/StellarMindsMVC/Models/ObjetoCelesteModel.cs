namespace StellarMindsMVC.Models;

public class ObjetoCelesteModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public TipoObjetoCeleste Tipo { get; set; }
    public decimal Magnitud { get; set; }
}
