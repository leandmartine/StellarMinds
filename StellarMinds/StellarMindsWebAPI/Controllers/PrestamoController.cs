using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Excepciones;

namespace StellarMindsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PrestamoController : ControllerBase
{
    private IAltaPrestamo _altaCU;
    private IBajaPrestamo _bajaCU;
    private IObtenerPrestamos _obtenerTodosCU;
    private IObtenerPrestamosPorSocio _porSocioCU;
    private IObtenerPrestamosPorPeriodo _porPeriodoCU;
    private IObtenerSociosPorTelescopio _sociosPorTelCU;

    public PrestamoController(
        IAltaPrestamo altaCU,
        IBajaPrestamo bajaCU,
        IObtenerPrestamos obtenerTodosCU,
        IObtenerPrestamosPorSocio porSocioCU,
        IObtenerPrestamosPorPeriodo porPeriodoCU,
        IObtenerSociosPorTelescopio sociosPorTelCU)
    {
        _altaCU = altaCU;
        _bajaCU = bajaCU;
        _obtenerTodosCU = obtenerTodosCU;
        _porSocioCU = porSocioCU;
        _porPeriodoCU = porPeriodoCU;
        _sociosPorTelCU = sociosPorTelCU;
    }

    // Lista todos los préstamos (lo usa la pantalla de devolución del coordinador).
    [HttpGet]
    [Authorize(Roles = "COORDINADOR")]
    public IActionResult GetAll()
    {
        try { return Ok(_obtenerTodosCU.ObtenerTodos()); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Detalle de un préstamo (RF11, vista de auditoría del administrador).
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult GetById(int id)
    {
        try { return Ok(_obtenerTodosCU.ObtenerPorId(id)); }
        catch (PrestamoException ex) { return NotFound(ex.Message); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Préstamos activos de un socio (RF05 devolución y RF07 alta de observación).
    [HttpGet("socio/{socioId}/activos")]
    [Authorize(Roles = "SOCIO,COORDINADOR")]
    public IActionResult GetActivosBySocio(int socioId)
    {
        try { return Ok(_porSocioCU.ObtenerActivosPorSocio(socioId)); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Préstamos de un socio en un mes/año dado (RF08). Atrasado marca los vencidos sin devolver.
    [HttpGet("periodo/{socioId}/{mes}/{anio}")]
    [Authorize(Roles = "SOCIO")]
    public IActionResult GetByPeriodo(int socioId, int mes, int anio)
    {
        try { return Ok(_porPeriodoCU.ObtenerPorPeriodo(socioId, mes, anio)); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Socios que tomaron un telescopio dado (RF09), sin repetir y ordenados por nombre desc.
    [HttpGet("socios-por-telescopio/{telescopioId}")]
    [Authorize(Roles = "ADMINISTRADOR,COORDINADOR")]
    public IActionResult GetSociosPorTelescopio(int telescopioId)
    {
        try { return Ok(_sociosPorTelCU.Obtener(telescopioId)); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Alta de préstamo (RF04). Valida reglas de negocio y deja la auditoría ALTA_PRESTAMO (RF06).
    [HttpPost]
    [Authorize(Roles = "COORDINADOR")]
    public IActionResult Post([FromBody] PrestamoAltaDTO dto)
    {
        try
        {
            _altaCU.Alta(dto);
            return Created("api/Prestamo", null);
        }
        catch (PrestamoException ex) { return BadRequest(ex.Message); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    // Devolución de préstamo (RF05): marca DEVUELTO, repone stock y audita la devolución (RF06).
    [HttpPost("{id}/devolver")]
    [Authorize(Roles = "COORDINADOR")]
    public IActionResult Devolver(int id, [FromQuery] int coordinadorId)
    {
        try
        {
            _bajaCU.Devolver(id, coordinadorId);
            return Ok();
        }
        catch (PrestamoException ex) { return BadRequest(ex.Message); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }
}
