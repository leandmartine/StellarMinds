namespace StellarMinds.InterfacesRepositorios;

using StellarMinds.Entidades;

// Contrato para persistir y consultar los registros de auditoría de préstamos (RF06).
// Es de solo escritura/lectura — no se editan ni eliminan auditorías.
public interface IRepositorioAuditoriaPrestamo
{
    void Registrar(AuditoriaPrestamo auditoria);
    IEnumerable<AuditoriaPrestamo> FindAll();
    IEnumerable<AuditoriaPrestamo> FindByPrestamo(int prestamoId);
}
