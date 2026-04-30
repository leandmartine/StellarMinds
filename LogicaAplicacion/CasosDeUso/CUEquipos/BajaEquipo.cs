using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

// RF03: la baja de un equipo no puede realizarse si está en préstamo activo.
public class BajaEquipo : IBajaEquipo
{
    private IRepositorioEquipo _repositorioEquipo;
    private IRepositorioPrestamo _repositorioPrestamo;

    // Inyectamos IRepositorioPrestamo para poder verificar si el equipo
    // está actualmente prestado antes de eliminarlo (principio DIP).
    public BajaEquipo(IRepositorioEquipo repositorioEquipo, IRepositorioPrestamo repositorioPrestamo)
    {
        _repositorioEquipo = repositorioEquipo;
        _repositorioPrestamo = repositorioPrestamo;
    }

    public void Baja(int id)
    {
        // Letra RF03 p.6: "deberá controlar que un equipo no esté en préstamo
        // para permitir el borrado, de lo contrario mostrará un mensaje descriptivo."
        if (_repositorioPrestamo.EquipoEnPrestamo(id))
            throw new EquipoException("No se puede eliminar el equipo porque está actualmente en préstamo.");

        _repositorioEquipo.Baja(id);
    }
}
