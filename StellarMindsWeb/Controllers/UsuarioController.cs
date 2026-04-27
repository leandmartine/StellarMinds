using DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.ValueObjets;
using StellarMindsWeb.Filtros;
using StellarMindsWeb.Models;

namespace StellarMindsWeb.Controllers;

public class UsuarioController : Controller
{
    private readonly IAltaUsuario altaUsuarioCU;
    private readonly IObtenerUsuarios findAllCU;
    private readonly ILoginUsuario loginUsuarioCU;
    private readonly ILogOutUsuario logOutUsuarioCU;

    public UsuarioController(
        IAltaUsuario altaCU,
        IObtenerUsuarios findAllCU,
        ILoginUsuario loginCU,
        ILogOutUsuario logOutCU)
    {
        this.altaUsuarioCU = altaCU;
        this.findAllCU = findAllCU;
        this.loginUsuarioCU = loginCU;
        this.logOutUsuarioCU = logOutCU;
    }

    // Listado de usuarios: solo admin.
    [Autorizacion(StellarMinds.Enums.RolDeUsuario.ADMINISTRADOR)]
    public IActionResult Index()
    {
        return View(findAllCU.ObtenerUsuarios());
    }

    // Alta de usuario (registro): accesible sin login, segun regla #2.
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CrearUsuarioViewModel vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new UsuarioDTO
            {
                NombreCompleto = new NombreCompleto(vm.NombreCompleto),
                Direccion     = new Direccion(vm.Calle, vm.Numero, vm.Apartamento, vm.Esquina, vm.Departamento, vm.Pais),
                Mail          = new Email(vm.Mail),
                Telefono      = vm.Telefono,
                NombreUsuario = vm.NombreUsuario,
                Contrasena    = new Contrasenha(vm.Contrasena),
                rol           = vm.Rol
            };

            altaUsuarioCU.AltaUsuario(dto);
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(vm);
        }
    }

    // Login: accesible sin login, segun regla #2.
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new UsuarioDTO
            {
                NombreUsuario = model.NombreUsuario,
                Contrasena    = new Contrasenha(model.Password)
            };

            // El CU devuelve el DTO con Id y rol para que podamos guardarlo en sesion.
            var logueado = loginUsuarioCU.LoginUsuario(dto);

            // Guardamos solo lo imprescindible. El rol lo vamos a resolver en el filtro
            // yendo a buscar el usuario por Id (punto 3 y 8 de tu feedback).
            HttpContext.Session.SetString("NombreUsuario", logueado.NombreUsuario);
            HttpContext.Session.SetInt32("IdUsuario", logueado.Id);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    // Logout: para poder ejecutarlo hay que estar logueado.
    [Autorizacion]
    public IActionResult Logout()
    {
        // Traemos el Id de la sesion para avisarle al caso de uso quien se esta
        // deslogueando. El caso de uso valida que exista y podria hacer logica extra
        // (auditoria, etc). La limpieza de la sesion es responsabilidad del controller.
        int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        if (idUsuario != null)
        {
            try { logOutUsuarioCU.LogOut(idUsuario.Value); }
            catch { /* si el usuario ya no existe, igual seguimos con el logout */ }
        }

        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
