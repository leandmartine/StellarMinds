using System.ComponentModel.DataAnnotations;
using DTOs;
using DTOs.DTOs;

namespace StellarMindsWeb.Models;

// ViewModel para el formulario de Alta de Préstamo (RF04).
// Contiene los datos que ingresa el coordinador + las listas para los dropdowns.
// Los campos CamaraId y OcularId son opcionales (nullable) porque la letra dice
// que al menos uno de los dos es obligatorio, pero no ambos.
public class PrestamoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un socio")]
    [Display(Name = "Socio")]
    public int SocioId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [Display(Name = "Fecha de inicio")]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    [Display(Name = "Fecha de fin")]
    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(7);

    [Required(ErrorMessage = "Debe seleccionar un telescopio")]
    [Display(Name = "Telescopio")]
    public int TelescopioId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una montura")]
    [Display(Name = "Montura")]
    public int MonturaId { get; set; }

    // Opcional: al menos uno de los dos (Cámara u Ocular) es requerido.
    // La validación de que al menos uno esté presente se hace en el servidor.
    [Display(Name = "Cámara (opcional)")]
    public int? CamaraId { get; set; }

    [Display(Name = "Ocular (opcional)")]
    public int? OcularId { get; set; }

    // --- Listas para los dropdowns (se cargan en el controller) ---
    // No se validan porque son listas de opciones, no datos del formulario.
    public List<UsuarioDTO> Socios { get; set; } = new();
    public List<EquipoDTO> Telescopios { get; set; } = new();
    public List<EquipoDTO> Monturas { get; set; } = new();
    public List<EquipoDTO> Camaras { get; set; } = new();
    public List<EquipoDTO> Oculares { get; set; } = new();
}
