namespace LogicaAccesoDatos.RepositorioMemoria;

using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

// Repositorio en memoria para AuditoriaPrestamo. No implementa IRepositorio<T>
// porque la auditoría no se edita ni se da de baja — solo se escribe y se lee.
// Esto respeta el principio ISP (Interface Segregation) de SOLID.
public class RepositorioAuditoriaPrestamo : IRepositorioAuditoriaPrestamo
{
    public static List<AuditoriaPrestamo> Auditorias = new List<AuditoriaPrestamo>();

    public void Registrar(AuditoriaPrestamo auditoria)
    {
        auditoria.Id = Auditorias.Count == 0 ? 1 : Auditorias.Max(a => a.Id) + 1;
        Auditorias.Add(auditoria);
    }

    public IEnumerable<AuditoriaPrestamo> FindAll()
    {
        return Auditorias;
    }

    // LINQ sintaxis de método: filtra auditorías de un préstamo específico.
    public IEnumerable<AuditoriaPrestamo> FindByPrestamo(int prestamoId)
    {
        return Auditorias
            .Where(a => a.Prestamo.Id == prestamoId)
            .ToList();
    }
}
