using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

public class EquipoController : Controller
{
    private readonly string _apiBaseUrl;

    public EquipoController(IConfiguration configuration)
    {
        _apiBaseUrl = $"{configuration["ApiBaseUrl"]!.TrimEnd('/')}/api/Equipo";
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Index()
    {
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl, VerbosHttp.GET, null, token);
        if (!respuesta.IsSuccessStatusCode)
            return View(new List<EquipoModel>());

        List<EquipoModel> equipos = JsonConvert.DeserializeObject<List<EquipoModel>>(ClienteHttpAuxiliar.ObtenerBody(respuesta));
        return View(equipos);
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Details(int id)
    {
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/" + id, VerbosHttp.GET, null, token);
        if (!respuesta.IsSuccessStatusCode)
        {
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
            return RedirectToAction(nameof(Index));
        }
        EquipoModel dto = JsonConvert.DeserializeObject<EquipoModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta));
        return View(dto);
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Create() => View(new EquipoViewModel());

    [HttpPost]
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Create(EquipoViewModel vm)
    {
        try
        {
            LimpiarModelStateSegunTipo(vm);
            if (!ModelState.IsValid) return View(vm);

            (string ruta, object payload) = ConstruirPayload(vm);
            if (ruta == null)
            {
                ViewBag.Error = "Tipo de equipo no válido.";
                return View(vm);
            }

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage r = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + ruta, VerbosHttp.POST, payload, token);

            if (r.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(r);
            return View(vm);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(vm);
        }
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Edit(int id)
    {
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/" + id, VerbosHttp.GET, null, token);
        if (!respuesta.IsSuccessStatusCode)
        {
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
            return RedirectToAction(nameof(Index));
        }
        EquipoModel dto = JsonConvert.DeserializeObject<EquipoModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta));
        return View(MapearModelAViewModel(dto));
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Edit(int id, EquipoViewModel vm)
    {
        try
        {
            if (id != vm.Id)
                throw new Exception("El Id del formulario no coincide con el de la URL");

            LimpiarModelStateSegunTipo(vm);
            if (!ModelState.IsValid) return View(vm);

            (string ruta, object payload) = ConstruirPayload(vm);
            if (ruta == null)
            {
                ViewBag.Error = "Tipo de equipo no válido.";
                return View(vm);
            }

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage r = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + ruta + "/" + id, VerbosHttp.PUT, payload, token);

            if (r.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(r);
            return View(vm);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(vm);
        }
    }

    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Delete(int id)
    {
        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/" + id, VerbosHttp.GET, null, token);
        if (!respuesta.IsSuccessStatusCode)
        {
            ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
            return RedirectToAction(nameof(Index));
        }
        EquipoModel dto = JsonConvert.DeserializeObject<EquipoModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta));
        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            string token = HttpContext.Session.GetString("token");

            // Verbo DELETE hacia la API — si el equipo tiene préstamo activo la API retorna 400 con JSON
            HttpResponseMessage r = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/" + id, VerbosHttp.DELETE, null, token);

            if (r.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Equipo eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Intentar leer el error como objeto JSON con datos del préstamo bloqueante
            string bodyError = ClienteHttpAuxiliar.ObtenerBody(r);
            try
            {
                // Si la API devolvió JSON estructurado, extraer prestamoId y tipoPrestamo
                dynamic errorObj = JsonConvert.DeserializeObject<dynamic>(bodyError);
                if (errorObj?.prestamoId != null)
                {
                    ViewBag.Error = (string)errorObj.mensaje;
                    ViewBag.PrestamoId = (int)errorObj.prestamoId;
                    ViewBag.TipoPrestamo = (string)errorObj.tipoPrestamo;
                }
                else
                {
                    ViewBag.Error = bodyError;
                }
            }
            catch
            {
                // Si no era JSON, mostrar el texto directo
                ViewBag.Error = bodyError;
            }

            // Volver a cargar el equipo para mostrar la vista Delete con los datos y el mensaje
            HttpResponseMessage resp = ClienteHttpAuxiliar.EnviarSolicitud(_apiBaseUrl + "/" + id, VerbosHttp.GET, null, token);
            if (resp.IsSuccessStatusCode)
                return View("Delete", JsonConvert.DeserializeObject<EquipoModel>(ClienteHttpAuxiliar.ObtenerBody(resp)));

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    private void LimpiarModelStateSegunTipo(EquipoViewModel vm)
    {
        if (vm.TipoEquipo != "TELESCOPIO")
        {
            ModelState.Remove(nameof(vm.Apertura));
            ModelState.Remove(nameof(vm.RelFocal));
            ModelState.Remove(nameof(vm.DistanciaFocal));
            ModelState.Remove(nameof(vm.Peso));
        }
        if (vm.TipoEquipo != "MONTURA")
        {
            ModelState.Remove(nameof(vm.TipoMontura));
            ModelState.Remove(nameof(vm.CargaSoportada));
            ModelState.Remove(nameof(vm.Computorizado));
        }
        if (vm.TipoEquipo != "CAMARA")
        {
            ModelState.Remove(nameof(vm.Sensor));
            ModelState.Remove(nameof(vm.Resolucion));
            ModelState.Remove(nameof(vm.Pixel));
        }
        if (vm.TipoEquipo != "OCULAR")
        {
            ModelState.Remove(nameof(vm.Diametro));
            ModelState.Remove(nameof(vm.Angulo));
        }
    }

    // Según el tipo seleccionado, arma el payload concreto (sin nullables) y la sub-ruta del endpoint.
    // El form usa un único EquipoViewModel con campos anulables; acá se traduce al modelo del subtipo.
    // Devuelve (null, null) si el tipo no es válido.
    private static (string ruta, object payload) ConstruirPayload(EquipoViewModel vm)
    {
        switch (vm.TipoEquipo)
        {
            case "TELESCOPIO":
                return ("/telescopio", new TelescopioAltaModel
                {
                    Marca = vm.Marca,
                    Modelo = vm.Modelo,
                    Stock = vm.Stock,
                    Apertura = vm.Apertura ?? 0,
                    RelFocal = vm.RelFocal,
                    DistanciaFocal = vm.DistanciaFocal ?? 0,
                    Peso = vm.Peso ?? 0
                });

            case "MONTURA":
                return ("/montura", new MonturaAltaModel
                {
                    Marca = vm.Marca,
                    Modelo = vm.Modelo,
                    Stock = vm.Stock,
                    TipoMontura = vm.TipoMontura ?? default,
                    CargaSoportada = vm.CargaSoportada ?? 0,
                    Computorizado = vm.Computorizado
                });

            case "CAMARA":
                return ("/camara", new CamaraAltaModel
                {
                    Marca = vm.Marca,
                    Modelo = vm.Modelo,
                    Stock = vm.Stock,
                    Sensor = vm.Sensor,
                    Resolucion = vm.Resolucion,
                    Pixel = vm.Pixel ?? 0
                });

            case "OCULAR":
                return ("/ocular", new OcularAltaModel
                {
                    Marca = vm.Marca,
                    Modelo = vm.Modelo,
                    Stock = vm.Stock,
                    Diametro = vm.Diametro ?? 0,
                    Angulo = vm.Angulo ?? 0
                });

            default:
                return (null, null);
        }
    }

    private static EquipoViewModel MapearModelAViewModel(EquipoModel dto) => new()
    {
        Id = dto.Id,
        TipoEquipo = dto.TipoEquipo,
        Marca = dto.Marca,
        Modelo = dto.Modelo,
        Stock = dto.Stock,
        Apertura = dto.Apertura,
        RelFocal = dto.RelFocal,
        DistanciaFocal = dto.DistanciaFocal,
        Peso = dto.Peso,
        TipoMontura = dto.TipoMontura,
        CargaSoportada = dto.CargaSoportada,
        Computorizado = dto.Computorizado ?? false,
        Sensor = dto.Sensor,
        Resolucion = dto.Resolucion,
        Pixel = dto.Pixel,
        Diametro = dto.Diametro,
        Angulo = dto.Angulo
    };
}
