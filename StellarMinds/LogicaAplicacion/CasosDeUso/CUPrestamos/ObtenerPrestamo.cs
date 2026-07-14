using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

// Caso de uso de consulta de préstamos: todos o por ID.

public class ObtenerPrestamo : IObtenerPrestamos
{
    private readonly IRepositorioPrestamo _repoPrestamo;


    public ObtenerPrestamo(IRepositorioPrestamo repoPrestamo)
    {
        _repoPrestamo = repoPrestamo;
    }

    public IEnumerable<PrestamoDTO> ObtenerTodos() =>
        _repoPrestamo.FindAll().Select(MapToDto);

    public PrestamoDTO ObtenerPorId(int id) => MapToDto(_repoPrestamo.FindById(id));

    // Mapea la entidad a DTO. Atrasado = sigue activo y la fecha de fin ya pasó.
    public static PrestamoDTO MapToDto(Prestamo p) => new PrestamoDTO
    {
        Id = p.Id,
        FechaInicio = p.FechaInicio,
        FechaFin = p.FechaFin,
        Estado = p.Estado,
        SocioId = p.Socio.Id,
        SocioNombre = p.Socio.NombreUsuario,
        TelescopioId = p.Telescopio.Id,
        TelescopioNombre = $"{p.Telescopio.Marca} {p.Telescopio.Modelo}",
        MonturaId = p.Montura.Id,
        MonturaNombre = $"{p.Montura.Marca} {p.Montura.Modelo}",
        CamaraId = p.Camara?.Id,
        CamaraNombre = p.Camara != null ? $"{p.Camara.Marca} {p.Camara.Modelo}" : null,
        OcularId = p.Ocular?.Id,
        OcularNombre = p.Ocular != null ? $"{p.Ocular.Marca} {p.Ocular.Modelo}" : null,
        // Atrasado: estado activo (EN_PRESTAMO) y la fecha de fin ya pasó
        Atrasado = p.Estado == EstadoPrestamo.PRESTAMO && p.FechaFin < DateTime.Today,

        TelescopioApertura = p.Telescopio.Apertura,
        TelescopioFocal = p.Telescopio.DistanciaFocal,
        TelescopioRelFocal = p.Telescopio.RelFocal,
        CamaraSensor = p.Camara?.Sensor,
        CamaraResolucion = p.Camara?.Resolucion,
        CamaraPixel = p.Camara?.Pixel,
        OcularDiametro = p.Ocular?.Diametro,
        OcularCampo = p.Ocular?.Angulo
    };
}
