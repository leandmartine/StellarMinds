using StellarMinds.Enums;
using StellarMinds.ValueObjets;

namespace DTOs;

public class UsuarioDTO
{
    public int Id {get; set;}
    public NombreCompleto NombreCompleto {get; set;}
    public Direccion Direccion {get; set;}
    public string Telefono {get; set;}
    public Email Mail {get; set;}
    public string NombreUsuario {get; set;}
    public Contrasenha Contrasena {get; set;}
    public RolDeUsuario rol {get; set;}
    
}