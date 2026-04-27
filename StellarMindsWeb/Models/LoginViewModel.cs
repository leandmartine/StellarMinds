using System.ComponentModel.DataAnnotations;

namespace StellarMindsWeb.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
}