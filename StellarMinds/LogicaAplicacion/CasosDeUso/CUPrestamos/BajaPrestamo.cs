using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

public class BajaPrestamo : IBajaPrestamo
{
    private readonly IRepositorioPrestamo _repoPrestamo;
    private readonly IRepositorioUsuario _repoUsuario;
    private readonly IRepositorioAuditoriaPrestamo _repoAuditoria;

    public BajaPrestamo(
        IRepositorioPrestamo repoPrestamo,
        IRepositorioUsuario repoUsuario,
        IRepositorioAuditoriaPrestamo repoAuditoria)
    {
        _repoPrestamo = repoPrestamo;
        _repoUsuario = repoUsuario;
        _repoAuditoria = repoAuditoria;
    }

    public void Devolver(int prestamoId, int coordinadorId)
    {
        Prestamo prestamo = _repoPrestamo.FindById(prestamoId);

        if (prestamo.Estado == EstadoPrestamo.DEVUELTO)
            throw new PrestamoException("El préstamo ya fue devuelto");

        prestamo.Estado = EstadoPrestamo.DEVUELTO;

        prestamo.Telescopio.Stock++;
        prestamo.Montura.Stock++;
        if (prestamo.Camara != null) prestamo.Camara.Stock++;
        if (prestamo.Ocular != null) prestamo.Ocular.Stock++;

        _repoPrestamo.Modificar(prestamo);

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
