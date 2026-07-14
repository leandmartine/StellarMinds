using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

public class PrestamoController : Controller
{
    private readonly string _apiBaseUrl;
    private readonly string _equipoBaseUrl;
    private readonly string _usuarioBaseUrl;

    public PrestamoController(IConfiguration configuration)
    {
        string baseUrl = configuration["ApiBaseUrl"]!.TrimEnd('/');
        _apiBaseUrl = $"{baseUrl}/api/Prestamo";
        _equipoBaseUrl = $"{baseUrl}/api/Equipo";
        _usuarioBaseUrl = $"{baseUrl}/api/Usuario";
    }

    // RF04 - Alta préstamo (COORDINADOR)
    [Autorizacion(RolDeUsuario.COORDINADOR)]
    public IActionResult Create()
    {
        PrestamoAltaViewModel vm = new PrestamoAltaViewModel();
        CargarListas(vm);
        return View(vm);
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.COORDINADOR)]
    public IActionResult Create(PrestamoAltaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            CargarListas(vm);
            return View(vm);
        }

        int coordinadorId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");

        PrestamoAltaModel dto = new PrestamoAltaModel
        {
            FechaInicio = vm.FechaInicio,
            FechaFin = vm.FechaFin,
            SocioId = vm.SocioId,
            CoordinadorId = coordinadorId,
            TelescopioId = vm.TelescopioId,
            MonturaId = vm.MonturaId,
            CamaraId = vm.CamaraId,
            OcularId = vm.OcularId
        };

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl, VerbosHttp.POST, dto, token);

        if (respuesta.IsSuccessStatusCode)
            return RedirectToAction(nameof(Devolucion));

        ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
        CargarListas(vm);
        return View(vm);
    }

    // RF05 - Devolución (COORDINADOR): se elige el socio y se listan sus préstamos pendientes.
    [Autorizacion(RolDeUsuario.COORDINADOR)]
    public IActionResult Devolucion(int? socioId)
    {
        string token = HttpContext.Session.GetString("token");
        DevolucionViewModel vm = new DevolucionViewModel { Socios = ObtenerSocios(token) };

        if (socioId.HasValue && socioId.Value > 0)
        {
            vm.SocioId = socioId.Value;
            vm.Buscado = true;

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                _apiBaseUrl + $"/socio/{socioId.Value}/activos", VerbosHttp.GET, null, token);

            if (respuesta.IsSuccessStatusCode)
                vm.PrestamosActivos = JsonConvert.DeserializeObject<IEnumerable<PrestamoModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? Enumerable.Empty<PrestamoModel>();
            else
                ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
        }

        return View(vm);
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.COORDINADOR)]
    public IActionResult Devolver(int prestamoId, int socioId)
    {
        int coordinadorId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl + $"/{prestamoId}/devolver?coordinadorId={coordinadorId}", VerbosHttp.POST, null, token);

        if (respuesta.IsSuccessStatusCode)
            TempData["Exito"] = $"El préstamo #{prestamoId} fue devuelto correctamente.";
        else
            TempData["Error"] = ClienteHttpAuxiliar.ObtenerBody(respuesta);

        return RedirectToAction(nameof(Devolucion), new { socioId });
    }

    // RF08 - Mis préstamos (SOCIO)
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult MisPrestamos()
    {
        return View(new MisPrestamosViewModel());
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult MisPrestamos(MisPrestamosViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        int socioId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl + $"/periodo/{socioId}/{vm.Mes}/{vm.Anio}", VerbosHttp.GET, null, token);

        if (respuesta.IsSuccessStatusCode)
            vm.Prestamos = JsonConvert.DeserializeObject<IEnumerable<PrestamoModel>>(ClienteHttpAuxiliar.ObtenerBody(respuesta))
                          ?? Enumerable.Empty<PrestamoModel>();
        else
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);

        vm.Buscado = true;
        return View(vm);
    }

    // RF09 - Socios por telescopio (COORDINADOR y ADMINISTRADOR)
    [Autorizacion(RolDeUsuario.COORDINADOR, RolDeUsuario.ADMINISTRADOR)]
    public IActionResult SociosPorTelescopio()
    {
        SociosPorTelescopioViewModel vm = new SociosPorTelescopioViewModel();
        CargarTelescopios(vm);
        return View(vm);
    }

    [HttpPost]
    // RF09: tanto COORDINADOR como ADMINISTRADOR pueden consultar socios por telescopio
    [Autorizacion(RolDeUsuario.COORDINADOR, RolDeUsuario.ADMINISTRADOR)]
    public IActionResult SociosPorTelescopio(SociosPorTelescopioViewModel vm)
    {
        CargarTelescopios(vm);
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl + $"/socios-por-telescopio/{vm.TelescopioId}", VerbosHttp.GET, null, token);
        if (respuesta.IsSuccessStatusCode)
            vm.Socios = JsonConvert.DeserializeObject<IEnumerable<UsuarioResumenModel>>(ClienteHttpAuxiliar.ObtenerBody(respuesta))
                       ?? Enumerable.Empty<UsuarioResumenModel>();
        else
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);

        vm.Buscado = true;
        return View(vm);
    }

    private void CargarListas(PrestamoAltaViewModel vm)
    {
        string token = HttpContext.Session.GetString("token");
        IEnumerable<EquipoModel> equipos = ObtenerEquipos(token);
        IEnumerable<UsuarioResumenModel> usuarios = ObtenerUsuarios(token);

        vm.Socios = usuarios
            .Where(u => u.Rol == RolDeUsuario.SOCIO)
            .Select(u => new SelectListItem($"{u.NombreCompleto} ({u.NombreUsuario})", u.Id.ToString()));

        vm.Telescopios = equipos
            .Where(e => e.TipoEquipo == "TELESCOPIO")
            .Select(e => new SelectListItem($"{e.Marca} {e.Modelo} (stock: {e.Stock})", e.Id.ToString()));

        vm.Monturas = equipos
            .Where(e => e.TipoEquipo == "MONTURA")
            .Select(e => new SelectListItem($"{e.Marca} {e.Modelo} (stock: {e.Stock})", e.Id.ToString()));

        vm.Camaras = equipos
            .Where(e => e.TipoEquipo == "CAMARA")
            .Select(e => new SelectListItem($"{e.Marca} {e.Modelo} (stock: {e.Stock})", e.Id.ToString()));

        vm.Oculares = equipos
            .Where(e => e.TipoEquipo == "OCULAR")
            .Select(e => new SelectListItem($"{e.Marca} {e.Modelo} (stock: {e.Stock})", e.Id.ToString()));
    }

    private void CargarTelescopios(SociosPorTelescopioViewModel vm)
    {
        string token = HttpContext.Session.GetString("token");
        vm.Telescopios = ObtenerEquipos(token)
            .Where(e => e.TipoEquipo == "TELESCOPIO")
            .Select(e => new SelectListItem($"{e.Marca} {e.Modelo}", e.Id.ToString()));
    }

    private IEnumerable<EquipoModel> ObtenerEquipos(string token)
    {
        HttpResponseMessage resp = ClienteHttpAuxiliar.EnviarSolicitud(_equipoBaseUrl, VerbosHttp.GET, null, token);
        if (!resp.IsSuccessStatusCode) return Enumerable.Empty<EquipoModel>();
        return JsonConvert.DeserializeObject<IEnumerable<EquipoModel>>(ClienteHttpAuxiliar.ObtenerBody(resp))
               ?? Enumerable.Empty<EquipoModel>();
    }

    private IEnumerable<UsuarioResumenModel> ObtenerUsuarios(string token)
    {
        HttpResponseMessage resp = ClienteHttpAuxiliar.EnviarSolicitud(_usuarioBaseUrl, VerbosHttp.GET, null, token);
        if (!resp.IsSuccessStatusCode) return Enumerable.Empty<UsuarioResumenModel>();
        return JsonConvert.DeserializeObject<IEnumerable<UsuarioResumenModel>>(ClienteHttpAuxiliar.ObtenerBody(resp))
               ?? Enumerable.Empty<UsuarioResumenModel>();
    }

    private IEnumerable<SelectListItem> ObtenerSocios(string token) =>
        ObtenerUsuarios(token)
            .Where(u => u.Rol == RolDeUsuario.SOCIO)
            .Select(u => new SelectListItem($"{u.NombreCompleto} ({u.NombreUsuario})", u.Id.ToString()));
}
