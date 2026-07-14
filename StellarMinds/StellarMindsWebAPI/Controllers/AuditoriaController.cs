using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StellarMindsWebAPI.Controllers;

// Auditoría de préstamos (RF11). Consulta con filtro opcional por coordinador.
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMINISTRADOR")]
public class AuditoriaController : ControllerBase
{
    private readonly IObtenerAuditoriaPrestamos _obtenerCU;

    public AuditoriaController(IObtenerAuditoriaPrestamos obtenerCU)
    {
        _obtenerCU = obtenerCU;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] int? coordinadorId = null)
    {
        try
        {
            var resultado = coordinadorId.HasValue
                ? _obtenerCU.ObtenerPorCoordinador(coordinadorId.Value)
                : _obtenerCU.ObtenerTodas();

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
