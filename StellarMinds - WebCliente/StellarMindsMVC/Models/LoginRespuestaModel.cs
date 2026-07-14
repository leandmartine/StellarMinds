namespace StellarMindsMVC.Models;

public class LoginRespuestaModel
{
    public string Token { get; set; }
    public UsuarioSesionModel Usuario { get; set; }
}

public class UsuarioSesionModel
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; }
    public RolDeUsuario Rol { get; set; }
}
