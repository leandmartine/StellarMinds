using DTOs;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Enums;
using StellarMindsWeb.Filtros;
using StellarMindsWeb.Models;

namespace StellarMindsWeb.Controllers;

// RF04 y RF05 son exclusivos del Coordinador (letra p.6).
[Autorizacion(RolDeUsuario.COORDINADOR)]
public class PrestamoController : Controller
{
    private readonly IAltaPrestamo _altaPrestamo;
    private readonly IDevolucionPrestamo _devolucion;
    private readonly IObtenerPrestamosPorUsuario _obtenerPorUsuario;
    private readonly IObtenerUsuarios _obtenerUsuarios;
    private readonly IObtenerEquipos _obtenerEquipos;

    // Constructor con inyección de dependencias (principio DIP de SOLID).
    public PrestamoController(
        IAltaPrestamo altaPrestamo,
        IDevolucionPrestamo devolucion,
        IObtenerPrestamosPorUsuario obtenerPorUsuario,
        IObtenerUsuarios obtenerUsuarios,
        IObtenerEquipos obtenerEquipos)
    {
        _altaPrestamo = altaPrestamo;
        _devolucion = devolucion;
        _obtenerPorUsuario = obtenerPorUsuario;
        _obtenerUsuarios = obtenerUsuarios;
        _obtenerEquipos = obtenerEquipos;
    }

    // =========================================================
    // RF04 – Alta de préstamo
    // =========================================================

    // GET /Prestamo/Create
    // Muestra el formulario vacío con todos los dropdowns cargados.
    public IActionResult Create()
    {
        return View(CargarDropdowns(new PrestamoViewModel()));
    }

    // POST /Prestamo/Create
    // Procesa el formulario. El coordinadorId viene de la sesión (RF06).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PrestamoViewModel vm)
    {
        // Validación custom: al menos cámara u ocular (la letra p.3 lo exige).
        if (vm.CamaraId == null && vm.OcularId == null)
            ModelState.AddModelError(string.Empty, "Debe seleccionar al menos una cámara o un ocular.");

        if (!ModelState.IsValid)
            return View(CargarDropdowns(vm));

        try
        {
            // El coordinador es el usuario logueado (sesión).
            int coordinadorId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            PrestamoDTO dto = new PrestamoDTO
            {
                FechaInicio = vm.FechaInicio,
                FechaFin = vm.FechaFin,
                SocioId = vm.SocioId,
                TelescopioId = vm.TelescopioId,
                MonturaId = vm.MonturaId,
                CamaraId = vm.CamaraId,
                OcularId = vm.OcularId
            };

            _altaPrestamo.Alta(dto, coordinadorId);
            TempData["Exito"] = "Préstamo registrado correctamente.";
            return RedirectToAction(nameof(Create));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(CargarDropdowns(vm));
        }
    }

    // =========================================================
    // RF05 – Devolución de préstamo
    // =========================================================

    // GET /Prestamo/Devolucion
    // Paso 1: muestra la lista de todos los socios para que el coordinador elija.
    public IActionResult Devolucion()
    {
        DevolucionViewModel vm = new DevolucionViewModel
        {
            // Filtramos solo los usuarios con rol SOCIO para el dropdown.
            Socios = _obtenerUsuarios.ObtenerUsuarios()
                .Where(u => u.rol == RolDeUsuario.SOCIO)
                .ToList()
        };
        return View(vm);
    }

    // POST /Prestamo/Devolucion
    // Paso 2: carga los préstamos activos del socio seleccionado.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Devolucion(DevolucionViewModel vm)
    {
        vm.Socios = _obtenerUsuarios.ObtenerUsuarios()
            .Where(u => u.rol == RolDeUsuario.SOCIO)
            .ToList();

        if (vm.SocioSeleccionadoId == 0)
        {
            ViewBag.Error = "Debe seleccionar un socio.";
            return View(vm);
        }

        vm.PrestamosActivos = _obtenerPorUsuario
            .ObtenerActivosPorUsuario(vm.SocioSeleccionadoId)
            .ToList();

        if (!vm.PrestamosActivos.Any())
            ViewBag.Info = "El socio seleccionado no tiene préstamos activos para devolver.";

        return View(vm);
    }

    // POST /Prestamo/ConfirmarDevolucion
    // Paso 3: procesa la devolución del préstamo seleccionado.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmarDevolucion(int prestamoId)
    {
        try
        {
            int coordinadorId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            _devolucion.Devolver(prestamoId, coordinadorId);
            TempData["Exito"] = "Devolución registrada correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Devolucion));
    }

    // =========================================================
    // Helper: carga las listas de los dropdowns en el ViewModel
    // =========================================================
    private PrestamoViewModel CargarDropdowns(PrestamoViewModel vm)
    {
        List<EquipoDTO> equipos = _obtenerEquipos.ObtenerTodos();

        // Filtramos por TipoEquipo (usando LINQ con sintaxis de método, letra p.7).
        vm.Telescopios = equipos.Where(e => e.TipoEquipo == "TELESCOPIO").ToList();
        vm.Monturas = equipos.Where(e => e.TipoEquipo == "MONTURA").ToList();
        vm.Camaras = equipos.Where(e => e.TipoEquipo == "CAMARA").ToList();
        vm.Oculares = equipos.Where(e => e.TipoEquipo == "OCULAR").ToList();

        // Solo los usuarios con rol SOCIO pueden pedir prestamos.
        vm.Socios = _obtenerUsuarios.ObtenerUsuarios()
            .Where(u => u.rol == RolDeUsuario.SOCIO)
            .ToList();

        return vm;
    }
}
