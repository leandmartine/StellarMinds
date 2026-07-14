using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUEquipos;

public class BajaEquipo : IBajaEquipo
{
    private readonly IRepositorioEquipo _repositorioEquipo;
    private readonly IRepositorioPrestamo _repositorioPrestamo;

    public BajaEquipo(IRepositorioEquipo repositorioEquipo, IRepositorioPrestamo repositorioPrestamo)
    {
        _repositorioEquipo = repositorioEquipo;
        _repositorioPrestamo = repositorioPrestamo;
    }

    public void Baja(int id)
    {
        List<Prestamo> prestamos = _repositorioPrestamo
            .FindAll()
            .Where(p => p.Telescopio.Id == id ||
                        p.Montura.Id == id ||
                        (p.Camara != null && p.Camara.Id == id) ||
                        (p.Ocular != null && p.Ocular.Id == id))
            .ToList();

        // 1) Verificar préstamo ACTIVO: se informa el id y tipo para que el front muestre el enlace al detalle
        Prestamo prestamoActivo = prestamos.FirstOrDefault(p => p.Estado == EstadoPrestamo.PRESTAMO);
        if (prestamoActivo != null)
        {
            // Determinar si el préstamo activo está vencido (atrasado) o en curso normal
            string tipo = prestamoActivo.FechaFin < DateTime.Today ? "ATRASADO" : "EN PRÉSTAMO";
            throw new EquipoException(
                "No se puede eliminar un equipo que está actualmente en préstamo.",
                prestamoActivo.Id,
                tipo);
        }

        // 2) Verificar historial: si el equipo tiene préstamos devueltos no se puede eliminar
        // porque las FKs usan NoAction y SQL Server rechazaría el DELETE físico,
        // además de que eliminar equipos con historial rompería la trazabilidad de auditoría
        if (prestamos.Any())
            throw new EquipoException(
                "No se puede eliminar un equipo que tiene historial de préstamos. " +
                "La integridad de los datos de auditoría debe preservarse.");

        _repositorioEquipo.Baja(id);
    }
}
