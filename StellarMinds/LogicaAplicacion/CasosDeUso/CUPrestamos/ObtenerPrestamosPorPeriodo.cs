using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

public class ObtenerPrestamosPorPeriodo : IObtenerPrestamosPorPeriodo
{
    private readonly IRepositorioPrestamo _repoPrestamo;

    public ObtenerPrestamosPorPeriodo(IRepositorioPrestamo repoPrestamo)
    {
        _repoPrestamo = repoPrestamo;
    }

    public IEnumerable<PrestamoDTO> ObtenerPorPeriodo(int socioId, int mes, int anio) =>
        _repoPrestamo.FindBySocioAndPeriod(socioId, mes, anio).Select(ObtenerPrestamo.MapToDto);
}
