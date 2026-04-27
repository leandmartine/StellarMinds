using System.ComponentModel.DataAnnotations;
using StellarMinds.Enums;

namespace StellarMindsWeb.Models;

public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "La calle es obligatoria")]
    [Display(Name = "Calle")]
    public string Calle { get; set; }

    [Required(ErrorMessage = "El número es obligatorio")]
    [Display(Name = "Número")]
    public int Numero { get; set; }

    [Display(Name = "Apartamento")]
    public int Apartamento { get; set; }

    [Required(ErrorMessage = "La esquina es obligatoria")]
    [Display(Name = "Esquina")]
    public string Esquina { get; set; }

    [Required(ErrorMessage = "El departamento es obligatorio")]
    [Display(Name = "Departamento")]
    public string Departamento { get; set; }

    [Required(ErrorMessage = "El país es obligatorio")]
    [Display(Name = "País")]
    public string Pais { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [Display(Name = "Email")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Display(Name = "Nombre de usuario")]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio")]
    [Display(Name = "Rol")]
    public RolDeUsuario Rol { get; set; }
}
