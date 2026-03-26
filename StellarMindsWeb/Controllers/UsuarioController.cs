using DTOs;
using LogicaAplicacion.CasosDeUso.CUUsuario;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Entidades;
using StellarMinds.Enums;
using StellarMinds.InterfacesRepositorios;

namespace StellarMindsWeb.Controllers;

public class UsuarioController : Controller
{
    private IAltaUsuario altaUsuarioCU;
    private IObtenerUsuarios findAllCU;

    public UsuarioController(IAltaUsuario altaCU, IObtenerUsuarios findAllCU)
    {
        this.altaUsuarioCU = altaCU;
        this.findAllCU = findAllCU;
    }

    public IActionResult Index()
    {
        return View(findAllCU.ObtenerUsuarios());
    }

    public IActionResult Details(int? id)
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(UsuarioDTO aAgregar)
    {
        try
        {
            altaUsuarioCU.AltaUsuario(aAgregar);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

}