using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

// Auditoría de préstamos (RF11). Solo Administrador. Listado filtrable por coordinador + detalle del préstamo.
public class AuditoriaController : Controller
{
    private readonly string _apiAuditoriaUrl;
    private readonly string _apiUsuarioUrl;
    private readonly string _apiPrestamoUrl;

    public AuditoriaController(IConfiguration configuration)
    {
        string baseUrl = configuration["ApiBaseUrl"]!.TrimEnd('/');
        _apiAuditoriaUrl = $"{baseUrl}/api/Auditoria";
        _apiUsuarioUrl = $"{baseUrl}/api/Usuario";
        _apiPrestamoUrl = $"{baseUrl}/api/Prestamo";
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Index(int? coordinadorId = null)
    {
        string token = HttpContext.Session.GetString("token");

        string urlAuditoria = coordinadorId.HasValue
            ? $"{_apiAuditoriaUrl}?coordinadorId={coordinadorId.Value}"
            : _apiAuditoriaUrl;

        HttpResponseMessage respAuditoria = ClienteHttpAuxiliar.EnviarSolicitud(urlAuditoria, VerbosHttp.GET, null, token);
        HttpResponseMessage respUsuarios = ClienteHttpAuxiliar.EnviarSolicitud(_apiUsuarioUrl, VerbosHttp.GET, null, token);

        AuditoriaViewModel vm = new AuditoriaViewModel { CoordinadorIdFiltro = coordinadorId };

        if (respAuditoria.IsSuccessStatusCode)
        {
            vm.Auditorias = JsonConvert.DeserializeObject<IEnumerable<AuditoriaModel>>(
                ClienteHttpAuxiliar.ObtenerBody(respAuditoria)) ?? vm.Auditorias;
        }
        else
        {
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respAuditoria);
        }

        if (respUsuarios.IsSuccessStatusCode)
        {
            IEnumerable<UsuarioResumenModel> todosUsuarios =
                JsonConvert.DeserializeObject<IEnumerable<UsuarioResumenModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respUsuarios)) ?? [];

            vm.Coordinadores = todosUsuarios.Where(u => u.Rol == RolDeUsuario.COORDINADOR);
        }

        return View(vm);
    }

    // Detalle del préstamo asociado a un registro de auditoría (link "Ver préstamo").
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult DetallePrestamo(int prestamoId)
    {
        string token = HttpContext.Session.GetString("token");

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            $"{_apiPrestamoUrl}/{prestamoId}", VerbosHttp.GET, null, token);

        if (!respuesta.IsSuccessStatusCode)
        {
            TempData["Error"] = $"No se pudo obtener el préstamo #{prestamoId}: {ClienteHttpAuxiliar.ObtenerBody(respuesta)}";
            return RedirectToAction(nameof(Index));
        }

        PrestamoModel prestamo = JsonConvert.DeserializeObject<PrestamoModel>(
            ClienteHttpAuxiliar.ObtenerBody(respuesta));

        return View(prestamo);
    }
}
