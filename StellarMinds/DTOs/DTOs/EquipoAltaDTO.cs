using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs;

// Base abstracta para el alta/modificación de equipos.
// Cada subtipo concreto agrega solo SUS campos, todos no anulables y con su validación real.
// Así se elimina el [Required] implícito que el [ApiController] aplicaba a los campos de otros
// subtipos cuando se usaba un único EquipoDTO plano con propiedades string no anulables.
public abstract class EquipoAltaDTO
{
    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(50)]
    public string Marca { get; set; }

    [Required(ErrorMessage = "El modelo es obligatorio")]
    [StringLength(50)]
    public string Modelo { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }
}
