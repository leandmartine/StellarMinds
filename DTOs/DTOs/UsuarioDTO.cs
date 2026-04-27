using StellarMinds.Enums;
using StellarMinds.ValueObjets;
using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class UsuarioDTO
{
    public int Id {get; set;}

    [Display(Name = "Nombre completo")]
    public NombreCompleto NombreCompleto {get; set;}
    public Direccion Direccion {get; set;}

    [StringLength(9, MinimumLength = 9, ErrorMessage = "El telefono debe de tener 9 caracteres")]
    public string Telefono {get; set;}
    public Email Mail {get; set;}

    [StringLength(20, MinimumLength = 3, ErrorMessage = "Nombre de usuario debe de tener entre 3 y 20 caracteres")]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario {get; set;}
    [Display(Name = "Contraseña")]
    public Contrasenha Contrasena {get; set;}
    [Display(Name = "Rol")]
    public RolDeUsuario rol {get; set;}
    
}