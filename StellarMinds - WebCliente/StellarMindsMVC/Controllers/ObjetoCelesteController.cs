using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

// Ranking de objetos celestes (RF10), disponible para cualquier rol autenticado.
public class ObjetoCelesteController : Controller
{
    private readonly string _apiRankingUrl;

    public ObjetoCelesteController(IConfiguration configuration)
    {
        _apiRankingUrl = $"{configuration["ApiBaseUrl"]!.TrimEnd('/')}/api/ObjetoCeleste/ranking";
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR, RolDeUsuario.COORDINADOR, RolDeUsuario.SOCIO)]
    public IActionResult Ranking()
    {
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiRankingUrl, VerbosHttp.GET, null, token);

        IEnumerable<RankingObjetoCelesteModel> ranking = [];

        if (respuesta.IsSuccessStatusCode)
            ranking = JsonConvert.DeserializeObject<IEnumerable<RankingObjetoCelesteModel>>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? ranking;
        else
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);

        return View(ranking);
    }
}
