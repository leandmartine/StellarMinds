using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioAuditoriaPrestamoEF : IRepositorioAuditoriaPrestamo
{
    private readonly StellarMindsContext _context;

    public RepositorioAuditoriaPrestamoEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Registrar(AuditoriaPrestamo auditoria)
    {
        _context.AuditoriasPrestamo.Add(auditoria);
        _context.SaveChanges();
    }

    public IEnumerable<AuditoriaPrestamo> FindAll()
    {
        return _context.AuditoriasPrestamo
            .Include(a => a.Coordinador)
            .Include(a => a.Prestamo)
            .ToList();
    }

    public IEnumerable<AuditoriaPrestamo> FindByPrestamo(int prestamoId)
    {
        return _context.AuditoriasPrestamo
            .Include(a => a.Coordinador)
            .Include(a => a.Prestamo)
            .Where(a => a.Prestamo.Id == prestamoId)
            .ToList();
    }

    // RF11: filtra las auditorías por el coordinador que realizó la acción.
    public IEnumerable<AuditoriaPrestamo> FindByCoordinador(int coordinadorId)
    {
        return _context.AuditoriasPrestamo
            .Include(a => a.Coordinador)
            .Include(a => a.Prestamo)
            .Where(a => a.Coordinador.Id == coordinadorId)
            .ToList();
    }
}
