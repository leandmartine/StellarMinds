using StellarMinds.Entidades;

namespace StellarMinds.InterfacesRepositorios;


public interface IRepositorioAuditoriaPrestamo
{

    void Registrar(AuditoriaPrestamo auditoria);

    IEnumerable<AuditoriaPrestamo> FindAll();


    IEnumerable<AuditoriaPrestamo> FindByPrestamo(int prestamoId);

    IEnumerable<AuditoriaPrestamo> FindByCoordinador(int coordinadorId);
}
