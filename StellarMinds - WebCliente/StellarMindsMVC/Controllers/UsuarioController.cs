using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMindsMVC.Auxiliar;
using StellarMindsMVC.Enums;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

// Login/logout (RF01), alta de socios (RF02) y listado de usuarios.
public class UsuarioController : Controller
{
    private string _apiBaseUrl;

    public UsuarioController(IConfiguration configuration)
    {
        _apiBaseUrl = $"{configuration["ApiBaseUrl"]!.TrimEnd('/')}/api/Usuario";
    }


    private bool EstaLogueado() => !string.IsNullOrEmpty(HttpContext.Session.GetString("NombreUsuario"));

    [HttpGet]
    public IActionResult Login()
    {
        if (EstaLogueado())
            return RedirectToAction("Index", "Home");

        return View(new LoginModel());
    }

    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl + "/login", VerbosHttp.POST, model);

        if (respuesta.IsSuccessStatusCode)
        {
            LoginRespuestaModel loginRespuesta =
                JsonConvert.DeserializeObject<LoginRespuestaModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta));

            HttpContext.Session.SetString("token", loginRespuesta.Token);
            HttpContext.Session.SetInt32 ("IdUsuario", loginRespuesta.Usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", loginRespuesta.Usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", loginRespuesta.Usuario.Rol.ToString());

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Credenciales incorrectas. Verifique su usuario y contraseña.";
        return View(model);
    }


    // Listado de usuarios (solo Administrador).
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Index(string mensaje)
    {
        string token = HttpContext.Session.GetString("token");
        ViewBag.Mensaje = mensaje;

        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl, VerbosHttp.GET, null, token);

        if (respuesta.IsSuccessStatusCode)
        {
            IEnumerable<UsuarioResumenModel> usuarios =
                JsonConvert.DeserializeObject<IEnumerable<UsuarioResumenModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta));
            return View(usuarios);
        }

        ViewBag.Error = "No se pudieron obtener los usuarios.";
        return View(new List<UsuarioResumenModel>());
    }

    // Alta de socios/usuarios (RF02). Solo el Administrador puede registrar usuarios.
    [HttpGet]
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Create()
    {
        return View(new UsuarioModel());
    }

    [HttpPost]
    [Autorizacion(RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Create(UsuarioModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        string token = HttpContext.Session.GetString("token");
        HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
            _apiBaseUrl, VerbosHttp.POST, vm, token);

        if (respuesta.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index), new { mensaje = "Usuario creado correctamente" });

        ViewBag.Error = ClienteHttpAuxiliar.ObtenerBody(respuesta);
        return View(vm);
    }

    [Autorizacion]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
