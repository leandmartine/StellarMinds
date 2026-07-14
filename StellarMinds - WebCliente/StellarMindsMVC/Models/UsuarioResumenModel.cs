namespace StellarMindsMVC.Models;

public class UsuarioResumenModel
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string NombreUsuario { get; set; }
    public string Telefono { get; set; }
    public RolDeUsuario Rol { get; set; }
}
