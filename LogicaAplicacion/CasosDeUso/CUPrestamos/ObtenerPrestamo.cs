namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades.Enums;
using StellarMinds.InterfacesRepositorios;

// Caso de uso para consultar préstamos activos de un socio (RF05).
// Usa LINQ con sintaxis de método para mapear entidades a DTOs.
public class ObtenerPrestamo : IObtenerPrestamosPorUsuario
{
    private readonly IRepositorioPrestamo _repoPrestamo;

    public ObtenerPrestamo(IRepositorioPrestamo repoPrestamo)
    {
        _repoPrestamo = repoPrestamo;
    }

    // RF05: lista los préstamos EN_PRESTAMO del socio seleccionado.
    public IEnumerable<PrestamoDTO> ObtenerActivosPorUsuario(int socioId)
    {
        return _repoPrestamo
            .FindByUsuarioYEstado(socioId, EstadoPrestamo.PRESTAMO)
            .Select(p => new PrestamoDTO
            {
                Id = p.Id,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                Estado = p.Estado,
                SocioId = p.Socio.Id,
                SocioNombre = p.Socio.NombreCompleto.nombreCompleto,
                TelescopioId = p.Telescopio.Id,
                TelescopioDescripcion = $"{p.Telescopio.Marca} {p.Telescopio.Modelo}",
                MonturaId = p.Montura.Id,
                MonturaDescripcion = $"{p.Montura.Marca} {p.Montura.Modelo}",
                CamaraId = p.Camara?.Id,
                CamaraDescripcion = p.Camara != null ? $"{p.Camara.Marca} {p.Camara.Modelo}" : null,
                OcularId = p.Ocular?.Id,
                OcularDescripcion = p.Ocular != null ? $"{p.Ocular.Marca} {p.Ocular.Modelo}" : null
            })
            .ToList();
    }
}