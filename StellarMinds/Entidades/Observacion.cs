namespace StellarMinds.Entidades;

public class Observacion
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public Prestamo Prestamo { get; set; }
    public ObjetoCeleste Objeto { get; set; }
    public Usuario Socio { get; set; }


}