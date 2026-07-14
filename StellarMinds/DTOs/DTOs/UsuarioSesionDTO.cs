using StellarMinds.Enums;

namespace DTOs.DTOs;

public class UsuarioSesionDTO
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; }
    public RolDeUsuario Rol { get; set; }
}
