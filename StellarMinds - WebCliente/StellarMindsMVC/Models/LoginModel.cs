using System.ComponentModel.DataAnnotations;

namespace StellarMindsMVC.Models;

public class LoginModel
{
    [Required]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; }
}
