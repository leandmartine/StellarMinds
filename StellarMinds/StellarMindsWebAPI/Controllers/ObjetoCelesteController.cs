using LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StellarMindsWebAPI.Controllers;

// Objetos celestes: listado para el alta de observación (RF07) y ranking (RF10).
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ObjetoCelesteController : ControllerBase
{
    private IObtenerObjetosCelestes _obtenerCU;
    private IObtenerRankingObjetosCelestes _rankingCU;

    public ObjetoCelesteController(
        IObtenerObjetosCelestes obtenerCU,
        IObtenerRankingObjetosCelestes rankingCU)
    {
        _obtenerCU = obtenerCU;
        _rankingCU = rankingCU;
    }

    // Todos los objetos celestes, para poblar el combo del alta de observación (RF07).
    [HttpGet]
    public IActionResult GetAll()
    {
        try { return Ok(_obtenerCU.ObtenerTodos()); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // Ranking de los más observados (RF10). Cualquier rol autenticado.
    [HttpGet("ranking")]
    public IActionResult GetRanking()
    {
        try { return Ok(_rankingCU.ObtenerRanking()); }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }
}
