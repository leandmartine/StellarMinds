using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

public class ObservacionController : Controller
{
    private readonly string _apiBaseUrl;
    private readonly string _prestamoBaseUrl;
    private readonly string _objetoBaseUrl;

    public ObservacionController(IConfiguration configuration)
    {
        string baseUrl = configuration["ApiBaseUrl"]!.TrimEnd('/');
        _apiBaseUrl = $"{baseUrl}/api/Observacion";
        _prestamoBaseUrl = $"{baseUrl}/api/Prestamo";
        _objetoBaseUrl = $"{baseUrl}/api/ObjetoCeleste";
    }

    // Listado de observaciones del socio autenticado. Destino del botón "Ver las observaciones".
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult Index()
    {
        int socioId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");

        IEnumerable<ObservacionModel> observaciones = Enumerable.Empty<ObservacionModel>();

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl, VerbosHttp.GET, null, token);
        if (respuesta.IsSuccessStatusCode)
        {
            observaciones = JsonConvert.DeserializeObject<IEnumerable<ObservacionModel>>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? Enumerable.Empty<ObservacionModel>();
            // El socio solo ve sus propias observaciones
            observaciones = observaciones.Where(o => o.SocioId == socioId).ToList();
        }
        else
        {
            ViewBag.Error = "No se pudieron obtener las observaciones.";
        }

        return View(observaciones);
    }

    // RF07 - Alta observación (SOCIO)
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult Create()
    {
        ObservacionViewModel vm = new ObservacionViewModel();
        CargarListas(vm);
        return View(vm);
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult Create(ObservacionViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            CargarListas(vm);
            return View(vm);
        }

        int socioId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");

        ObservacionAltaModel dto = new ObservacionAltaModel
        {
            PrestamoId = vm.PrestamoId,
            ObjetoId = vm.ObjetoId,
            SocioId = socioId,
            Fecha = vm.Fecha,
            TipoObservacion = vm.TipoObservacion,
            // Reenvía el resultado del botón "Evaluar" para que la observación se guarde con esa evaluación
            Indicador = vm.IndicadorResultado,
            Detalle = vm.DetalleResultado
        };

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl, VerbosHttp.POST, dto, token);

        if (respuesta.IsSuccessStatusCode)
        {
            ObservacionModel creada = JsonConvert.DeserializeObject<ObservacionModel>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));

            if (!string.IsNullOrEmpty(creada?.Advertencia))
                // Se guardó igual, pero Gemini no pudo evaluar: se muestra el aviso por ViewBag
                ViewBag.Advertencia = creada.Advertencia;
            else
                ViewBag.Exito = "Observación registrada correctamente.";

            // Habilita el botón "Ver las observaciones" que redirige al listado
            ViewBag.MostrarVerObservaciones = true;

            ObservacionViewModel limpio = new ObservacionViewModel();
            CargarListas(limpio);
            return View(limpio);
        }

        ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
        CargarListas(vm);
        return View(vm);
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.SOCIO)]
    public IActionResult Evaluar(ObservacionViewModel vm)
    {
        if (vm.PrestamoId == 0 || vm.ObjetoId == 0)
        {
            ViewBag.Error = "Seleccione un préstamo y un objeto celeste antes de evaluar.";
            CargarListas(vm);
            return View("Create", vm);
        }

        int socioId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");

        ObservacionAltaModel dto = new ObservacionAltaModel
        {
            PrestamoId = vm.PrestamoId,
            ObjetoId = vm.ObjetoId,
            SocioId = socioId,
            Fecha = vm.Fecha == default ? DateTime.Today : vm.Fecha,
            TipoObservacion = vm.TipoObservacion
        };

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/evaluar", VerbosHttp.POST, dto, token);

        if (respuesta.IsSuccessStatusCode)
        {
            EvaluarResultadoModel resultado = JsonConvert.DeserializeObject<EvaluarResultadoModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta));
            vm.IndicadorResultado = resultado?.Indicador;
            vm.DetalleResultado = resultado?.Detalle;
        }
        else
        {
            ViewBag.Error = "No se pudo evaluar la adecuación.";
        }

        CargarListas(vm);
        return View("Create", vm);
    }

    private void CargarListas(ObservacionViewModel vm)
    {
        int socioId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        string token = HttpContext.Session.GetString("token");

        HttpResponseMessage respPrestamos = ClienteHttpAuxiliar.EnviarSolicitud(
            _prestamoBaseUrl + $"/socio/{socioId}/activos", VerbosHttp.GET, null, token);
        if (respPrestamos.IsSuccessStatusCode)
        {
            IEnumerable<PrestamoModel> prestamos = JsonConvert.DeserializeObject<IEnumerable<PrestamoModel>>(
                ClienteHttpAuxiliar.ObtenerBody(respPrestamos)) ?? Enumerable.Empty<PrestamoModel>();
            vm.Prestamos = prestamos.Select(p =>
                new SelectListItem($"Préstamo #{p.Id} — {p.TelescopioNombre} ({p.FechaInicio:dd/MM} al {p.FechaFin:dd/MM})", p.Id.ToString()));
        }

        HttpResponseMessage respObjetos = ClienteHttpAuxiliar.EnviarSolicitud(_objetoBaseUrl, VerbosHttp.GET, null, token);
        if (respObjetos.IsSuccessStatusCode)
        {
            IEnumerable<ObjetoCelesteModel> objetos = JsonConvert.DeserializeObject<IEnumerable<ObjetoCelesteModel>>(
                ClienteHttpAuxiliar.ObtenerBody(respObjetos)) ?? Enumerable.Empty<ObjetoCelesteModel>();
            vm.ObjetosCelestes = objetos.Select(o =>
                new SelectListItem($"{o.Nombre} ({o.Tipo}, mag {o.Magnitud})", o.Id.ToString()));
        }
    }
}

public class EvaluarResultadoModel
{
    public string Indicador { get; set; }
    public string Detalle { get; set; }
}
