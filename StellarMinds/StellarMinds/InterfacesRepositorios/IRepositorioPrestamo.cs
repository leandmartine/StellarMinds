namespace StellarMinds.InterfacesRepositorios;

using StellarMinds.Entidades;

public interface IRepositorioPrestamo : IRepositorio<Prestamo>
{
    IEnumerable<Prestamo> FindBySocio(int socioId);
    IEnumerable<Prestamo> FindActivosBySocio(int socioId);
    IEnumerable<Prestamo> FindBySocioAndPeriod(int socioId, int mes, int anio);
    IEnumerable<Prestamo> FindByTelescopio(int telescopioId);
    // Retorna todos los préstamos que involucren el equipo dado (cualquier tipo: telescopio, montura, cámara u ocular)
    IEnumerable<Prestamo> FindByEquipo(int equipoId);
}
