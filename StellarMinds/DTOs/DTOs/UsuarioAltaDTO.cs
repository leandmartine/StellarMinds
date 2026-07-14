using StellarMinds.Enums;

namespace DTOs.DTOs;

public class UsuarioAltaDTO
{
    public string NombreCompleto { get; set; }
    public string Calle { get; set; }
    public int Numero { get; set; }
    public int Apartamento { get; set; }
    public string Esquina { get; set; }
    public string Departamento { get; set; }
    public string Pais { get; set; }
    public string Telefono { get; set; }
    public string Mail { get; set; }
    public string NombreUsuario { get; set; }
    public string Contrasena { get; set; }
    public RolDeUsuario Rol { get; set; }
}
