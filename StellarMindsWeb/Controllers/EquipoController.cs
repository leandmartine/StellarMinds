using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Enums;
using StellarMindsWeb.Filtros;
using StellarMindsWeb.Models;

namespace StellarMindsWeb.Controllers;

// Todas las acciones del CRUD de equipos requieren rol ADMINISTRADOR (RF03).
[Autorizacion(RolDeUsuario.ADMINISTRADOR)]
public class EquipoController : Controller
{
    private readonly IAltaEquipo _altaCU;
    private readonly IModificarEquipo _modificarCU;
    private readonly IBajaEquipo _bajaCU;
    private readonly IObtenerEquipos _obtenerTodosCU;
    private readonly IObtenerEquipoPorId _obtenerPorIdCU;

    public EquipoController(
        IAltaEquipo altaCU,
        IModificarEquipo modificarCU,
        IBajaEquipo bajaCU,
        IObtenerEquipos obtenerTodosCU,
        IObtenerEquipoPorId obtenerPorIdCU)
    {
        _altaCU           = altaCU;
        _modificarCU      = modificarCU;
        _bajaCU           = bajaCU;
        _obtenerTodosCU   = obtenerTodosCU;
        _obtenerPorIdCU   = obtenerPorIdCU;
    }

    // GET /Equipo
    public IActionResult Index()
    {
        List<EquipoDTO> equipos = _obtenerTodosCU.ObtenerTodos();
        return View(equipos);
    }

    // GET /Equipo/Details/5
    public IActionResult Details(int id)
    {
        try
        {
            EquipoDTO dto = _obtenerPorIdCU.BuscarEquipoPorId(id);
            return View(dto);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // GET /Equipo/Create
    public IActionResult Create()
    {
        return View(new EquipoViewModel());
    }

    // POST /Equipo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EquipoViewModel vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);

            EquipoDTO dto = MapearViewModelADto(vm);
            _altaCU.Alta(dto);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(vm);
        }
    }

    // GET /Equipo/Edit/5
    public IActionResult Edit(int id)
    {
        try
        {
            EquipoDTO dto = _obtenerPorIdCU.BuscarEquipoPorId(id);
            return View(MapearDtoAViewModel(dto));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // POST /Equipo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EquipoViewModel vm)
    {
        try
        {
            if (id != vm.Id)
                throw new Exception("El Id del formulario no coincide con el de la URL");

            if (!ModelState.IsValid) return View(vm);

            EquipoDTO dto = MapearViewModelADto(vm);
            _modificarCU.Modificar(dto);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(vm);
        }
    }

    // GET /Equipo/Delete/5
    public IActionResult Delete(int id)
    {
        try
        {
            EquipoDTO dto = _obtenerPorIdCU.BuscarEquipoPorId(id);
            return View(dto);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // POST /Equipo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _bajaCU.Baja(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Intentamos volver a la vista Delete con el DTO cargado para mostrar el error.
            ViewBag.Error = ex.Message;
            try
            {
                return View("Delete", _obtenerPorIdCU.BuscarEquipoPorId(id));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }

    // --- Helpers privados ---

    private EquipoDTO MapearViewModelADto(EquipoViewModel vm)
    {
        return new EquipoDTO
        {
            Id = vm.Id,
            TipoEquipo = vm.TipoEquipo,
            Marca = vm.Marca,
            Modelo = vm.Modelo,
            Stock = vm.Stock,
            // Telescopio
            Apertura = vm.Apertura,
            RelFocal = vm.RelFocal,
            DistanciaFocal = vm.DistanciaFocal,
            Peso = vm.Peso,
            // Montura
            TipoMontura = vm.TipoMontura,
            CargaSoportada = vm.CargaSoportada,
            Computorizado = vm.Computorizado,
            // Camara
            Sensor = vm.Sensor,
            Resolucion = vm.Resolucion,
            Pixel = vm.Pixel,
            // Ocular
            Diametro = vm.Diametro,
            Angulo = vm.Angulo
        };
    }

    private EquipoViewModel MapearDtoAViewModel(EquipoDTO dto)
    {
        return new EquipoViewModel
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
}
