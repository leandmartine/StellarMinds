using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Observaciones;
using LogicaAplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace StellarMindsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SOCIO")]
public class ObservacionController : ControllerBase
{
    private readonly IAltaObservacion _altaCU;
    private readonly IObtenerObservaciones _obtenerCU;
    private readonly IServicioGemini _gemini;
    private readonly IRepositorioPrestamo _repoPrestamo;
    private readonly IRepositorioObjetoCeleste _repoObjeto;

    public ObservacionController(
        IAltaObservacion altaCU,
        IObtenerObservaciones obtenerCU,
        IServicioGemini gemini,
        IRepositorioPrestamo repoPrestamo,
        IRepositorioObjetoCeleste repoObjeto)
    {
        _altaCU = altaCU;
        _obtenerCU = obtenerCU;
        _gemini = gemini;
        _repoPrestamo = repoPrestamo;
        _repoObjeto = repoObjeto;
    }

    // GET api/Observacion
    [HttpGet]
    public IActionResult GetAll()
    {
        try { return Ok(_obtenerCU.ObtenerTodas()); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // POST api/Observacion
    [HttpPost]
    public IActionResult Post([FromBody] ObservacionAltaDTO dto)
    {
        try
        {
            ObservacionDTO resultado = _altaCU.Alta(dto);
            return Created("api/Observacion", resultado);
        }
        catch (ObservacionException ex) { return BadRequest(ex.Message); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    // POST api/Observacion/evaluar  (solo evalua, no guarda)
    [HttpPost("evaluar")]
    public async Task<IActionResult> Evaluar([FromBody] ObservacionAltaDTO dto)
    {
        try
        {
            Prestamo prestamo = _repoPrestamo.FindById(dto.PrestamoId);
            ObjetoCeleste objeto = _repoObjeto.FindById(dto.ObjetoId);

            PrestamoDTO prestamoDto = LogicaAplicacion.CasosDeUso.CUPrestamos.ObtenerPrestamo.MapToDto(prestamo);
            ObjetoCelesteDTO objetoDto = new ObjetoCelesteDTO
            {
                Id = objeto.Id, Nombre = objeto.Nombre, Tipo = objeto.Tipo, Magnitud = objeto.Magnitud
            };

            ResultadoGemini resultado = await _gemini.EvaluarAsync(prestamoDto, objetoDto);
            return Ok(new { indicador = resultado.Indicador.ToString(), detalle = resultado.Detalle });
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
}
