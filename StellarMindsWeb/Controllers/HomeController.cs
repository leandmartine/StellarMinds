using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StellarMindsWeb.Filtros;
using StellarMindsWeb.Models;

namespace StellarMindsWeb.Controllers;

public class HomeController : Controller
{
    // Regla #2 del enunciado: todo lo que no sea Login ni Create(Usuario)
    // requiere sesion valida. Solo se pide estar logueado (sin rol especifico).
    [Autorizacion]
    public IActionResult Index()
    {
        return View();
    }

    [Autorizacion]
    public IActionResult Privacy()
    {
        return View();
    }

    // Error queda sin filtro para evitar loops cuando falla algo antes del login.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
