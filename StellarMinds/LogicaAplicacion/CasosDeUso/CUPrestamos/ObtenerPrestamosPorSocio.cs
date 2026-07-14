using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

public class ObtenerPrestamosPorSocio : IObtenerPrestamosPorSocio
{
    private readonly IRepositorioPrestamo _repoPrestamo;

    public ObtenerPrestamosPorSocio(IRepositorioPrestamo repoPrestamo)
    {
        _repoPrestamo = repoPrestamo;
    }

    public IEnumerable<PrestamoDTO> ObtenerActivosPorSocio(int socioId) =>
        _repoPrestamo.FindActivosBySocio(socioId).Select(ObtenerPrestamo.MapToDto);
}
