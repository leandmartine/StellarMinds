using DTOs;
using DTOs.DTOs;

namespace StellarMindsWeb.Models;

// ViewModel para el flujo de Devolución de Préstamo (RF05).
// El proceso tiene dos pasos: 1) elegir socio, 2) elegir préstamo de ese socio.
public class DevolucionViewModel
{
    // Paso 1: selección de socio
    public int SocioSeleccionadoId { get; set; }
    public List<UsuarioDTO> Socios { get; set; } = new();

    // Paso 2: lista de préstamos activos del socio seleccionado
    public List<PrestamoDTO> PrestamosActivos { get; set; } = new();
}
