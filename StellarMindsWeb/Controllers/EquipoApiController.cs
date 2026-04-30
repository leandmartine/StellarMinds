using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Enums;
using StellarMindsWeb.Filtros;

namespace StellarMindsWeb.Controllers;

// Endpoint WebAPI REST para verificación de disponibilidad de equipos.
// RF04 letra p.6: "deberá resolverse del lado del cliente y la información de si ese
// equipo está disponible deberá obtenerse consultando un endpoint WebAPI mediante javascript".
//
// Usamos [ApiController] para que ASP.NET serialice la respuesta como JSON automáticamente.
[ApiController]
[Route("api/[controller]")]
public class EquipoApiController : ControllerBase
{
    private readonly IObtenerEquipoPorId _obtenerPorId;

    public EquipoApiController(IObtenerEquipoPorId obtenerPorId)
    {
        _obtenerPorId = obtenerPorId;
    }

    // GET /api/equipoapi/{id}/disponible
    // Responde con JSON: { disponible: true/false, mensaje: "..." }
    // El JavaScript del formulario de Alta llama a este endpoint al seleccionar un equipo.
    [HttpGet("{id}/disponible")]
    public IActionResult VerificarDisponibilidad(int id)
    {
        try
        {
            var equipo = _obtenerPorId.BuscarEquipoPorId(id);

            if (equipo.Stock > 0)
            {
                // HTTP 200 con JSON indicando que hay stock.
                return Ok(new { disponible = true, mensaje = $"Stock disponible: {equipo.Stock}" });
            }
            else
            {
                // HTTP 200 con JSON indicando que no hay stock.
                // El JS usa el campo 'disponible' para mostrar el aviso sin enviar el form.
                return Ok(new { disponible = false, mensaje = $"Sin stock disponible: {equipo.Marca} {equipo.Modelo}" });
            }
        }
        catch (Exception ex)
        {
            // HTTP 404 si el equipo no existe (guía REST: recurso no encontrado).
            return NotFound(new { disponible = false, mensaje = ex.Message });
        }
    }
}
