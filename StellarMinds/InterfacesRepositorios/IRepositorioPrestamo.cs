namespace StellarMinds.InterfacesRepositorios;

using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;

// Hereda las operaciones CRUD genéricas (Alta, Modificar, Baja, FindAll, FindById)
// y agrega consultas propias de Prestamo.
public interface IRepositorioPrestamo : IRepositorio<Prestamo>
{
    // RF05: préstamos de un socio con estado EN_PRESTAMO para la devolución.
    IEnumerable<Prestamo> FindByUsuarioYEstado(int socioId, EstadoPrestamo estado);

    // RF03/RF04: verificar si un equipo está actualmente en préstamo.
    bool EquipoEnPrestamo(int equipoId);
}
