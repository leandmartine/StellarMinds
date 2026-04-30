namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

// RF05: procesamiento de la devolución de un préstamo.
// Cambia el estado a DEVUELTO y restaura el stock de los equipos devueltos.
// RF06: registra la devolución en la auditoría de forma automática.
public class DevolucionPrestamo : IDevolucionPrestamo
{
    private IRepositorioPrestamo _repoPrestamo;
    private IRepositorioEquipo _repoEquipo;
    private IRepositorioUsuario _repoUsuario;
    private IRepositorioAuditoriaPrestamo _repoAuditoria;

    public DevolucionPrestamo(
        IRepositorioPrestamo repoPrestamo,
        IRepositorioEquipo repoEquipo,
        IRepositorioUsuario repoUsuario,
        IRepositorioAuditoriaPrestamo repoAuditoria)
    {
        _repoPrestamo = repoPrestamo;
        _repoEquipo = repoEquipo;
        _repoUsuario = repoUsuario;
        _repoAuditoria = repoAuditoria;
    }

    public void Devolver(int prestamoId, int coordinadorId)
    {
        Prestamo prestamo = _repoPrestamo.FindById(prestamoId);

        // Solo se pueden devolver préstamos que están EN_PRESTAMO.
        if (prestamo.Estado != EstadoPrestamo.PRESTAMO)
            throw new PrestamoException("El préstamo seleccionado ya fue devuelto");

        // Cambiar estado a DEVUELTO (letra p.3).
        prestamo.Estado = EstadoPrestamo.DEVUELTO;

        // Restaurar stock: al devolver, la cantidad disponible vuelve a subir (letra p.3).
        prestamo.Telescopio.Stock++;
        prestamo.Montura.Stock++;
        if (prestamo.Camara != null) prestamo.Camara.Stock++;
        if (prestamo.Ocular != null) prestamo.Ocular.Stock++;

        // Persistir cambios en equipos y préstamo.
        _repoEquipo.Modificar(prestamo.Telescopio);
        _repoEquipo.Modificar(prestamo.Montura);
        if (prestamo.Camara != null) _repoEquipo.Modificar(prestamo.Camara);
        if (prestamo.Ocular != null) _repoEquipo.Modificar(prestamo.Ocular);
        _repoPrestamo.Modificar(prestamo);

        // --- RF06: registrar auditoría de devolución automáticamente ---
        Usuario coordinador = _repoUsuario.FindById(coordinadorId);
        AuditoriaPrestamo auditoria = new AuditoriaPrestamo
        {
            Accion = AccionAuditoria.DEVOLUCION,
            Fecha = DateTime.Now,
            Coordinador = coordinador,
            Prestamo = prestamo
        };
        _repoAuditoria.Registrar(auditoria);
    }
}
