using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StellarMindsMVC.Filtros;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Controllers;

public class HomeController : Controller
{
    /// <summary>
    /// Landing pública del proyecto (sin autenticación).
    /// </summary>
    public IActionResult Landing() => View();

    [Autorizacion]
    public IActionResult Index() => View();

    [Autorizacion]
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
