using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Excepciones;

namespace StellarMindsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EquipoController : ControllerBase
{
    private readonly IAltaEquipo _altaCU;
    private readonly IModificarEquipo _modificarCU;
    private readonly IBajaEquipo _bajaCU;
    private readonly IObtenerEquipos _obtenerTodosCU;
    private readonly IObtenerEquipoPorId _obtenerPorIdCU;

    public EquipoController(
        IAltaEquipo altaCU,
        IModificarEquipo modificarCU,
        IBajaEquipo bajaCU,
        IObtenerEquipos obtenerTodosCU,
        IObtenerEquipoPorId obtenerPorIdCU)
    {
        _altaCU = altaCU;
        _modificarCU = modificarCU;
        _bajaCU = bajaCU;
        _obtenerTodosCU = obtenerTodosCU;
        _obtenerPorIdCU = obtenerPorIdCU;
    }

    // GET api/Equipo
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,COORDINADOR")]
    public IActionResult GetAll()
    {
        return Ok(_obtenerTodosCU.ObtenerTodos());
    }

    // GET api/Equipo/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult GetById(int id)
    {
        try
        {
            return Ok(_obtenerPorIdCU.BuscarEquipoPorId(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST api/Equipo/telescopio
    [HttpPost("telescopio")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult AltaTelescopio([FromBody] TelescopioAltaDTO dto)
    {
        try
        {
            _altaCU.Alta(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/Equipo/montura
    [HttpPost("montura")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult AltaMontura([FromBody] MonturaAltaDTO dto)
    {
        try
        {
            _altaCU.Alta(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/Equipo/camara
    [HttpPost("camara")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult AltaCamara([FromBody] CamaraAltaDTO dto)
    {
        try
        {
            _altaCU.Alta(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/Equipo/ocular
    [HttpPost("ocular")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult AltaOcular([FromBody] OcularAltaDTO dto)
    {
        try
        {
            _altaCU.Alta(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/Equipo/telescopio/{id}
    [HttpPut("telescopio/{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult ModificarTelescopio(int id, [FromBody] TelescopioAltaDTO dto)
    {
        try
        {
            _modificarCU.Modificar(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/Equipo/montura/{id}
    [HttpPut("montura/{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult ModificarMontura(int id, [FromBody] MonturaAltaDTO dto)
    {
        try
        {
            _modificarCU.Modificar(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/Equipo/camara/{id}
    [HttpPut("camara/{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult ModificarCamara(int id, [FromBody] CamaraAltaDTO dto)
    {
        try
        {
            _modificarCU.Modificar(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/Equipo/ocular/{id}
    [HttpPut("ocular/{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult ModificarOcular(int id, [FromBody] OcularAltaDTO dto)
    {
        try
        {
            _modificarCU.Modificar(id, dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE api/Equipo/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public IActionResult Baja(int id)
    {
        try
        {
            _bajaCU.Baja(id);
            return Ok();
        }
        catch (EquipoException ex) when (ex.PrestamoId.HasValue)
        {
            // El equipo tiene un préstamo activo: se devuelve JSON con los datos del préstamo
            // para que el cliente MVC pueda mostrar un enlace al detalle del préstamo
            return BadRequest(new
            {
                mensaje = ex.Message,
                prestamoId = ex.PrestamoId.Value,
                tipoPrestamo = ex.TipoPrestamo
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET api/Equipo/{id}/disponibilidad
    // Anónimo: lo consulta el JavaScript del formulario de préstamo (RF04), que no adjunta el token.
    [HttpGet("{id}/disponibilidad")]
    [AllowAnonymous]
    public IActionResult Disponibilidad(int id)
    {
        try
        {
            EquipoDTO equipo = _obtenerPorIdCU.BuscarEquipoPorId(id);
            return Ok(new { disponible = equipo.Stock > 0, stock = equipo.Stock });
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
