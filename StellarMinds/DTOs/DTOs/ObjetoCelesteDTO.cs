using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class ObjetoCelesteDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public TipoObjetoCeleste Tipo { get; set; }
    public decimal Magnitud { get; set; }
}
