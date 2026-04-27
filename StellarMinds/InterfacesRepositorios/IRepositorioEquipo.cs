using StellarMinds.Entidades;

namespace StellarMinds.InterfacesRepositorios;

public interface IRepositorioEquipo : IRepositorio<Equipo>
{
    // Por ahora el CRUD generico alcanza. Si mas adelante necesitamos
    // consultas especificas de equipos (ej: equipos disponibles),
    // se agregan aca.
}
