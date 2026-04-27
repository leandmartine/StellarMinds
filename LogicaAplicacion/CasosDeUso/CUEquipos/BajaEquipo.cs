using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class BajaEquipo : IBajaEquipo
{
    private readonly IRepositorioEquipo _repositorioEquipo;

    public BajaEquipo(IRepositorioEquipo repositorioEquipo)
    {
        _repositorioEquipo = repositorioEquipo;
    }

    public void Baja(int id)
    {
        // NOTA: la letra pide que no se pueda dar de baja un equipo que este
        // en prestamo. Cuando se implemente RF04 (Alta de prestamo) se agrega
        // aca la inyeccion de IRepositorioPrestamo y el chequeo. Por ahora,
        // como no existen prestamos en el sistema, la baja se hace directa.
        _repositorioEquipo.Baja(id);
    }
}
