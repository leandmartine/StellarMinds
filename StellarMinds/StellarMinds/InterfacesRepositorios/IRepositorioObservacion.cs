namespace StellarMinds.InterfacesRepositorios;

using StellarMinds.Entidades;

public interface IRepositorioObservacion : IRepositorio<Observacion>
{
    IEnumerable<Observacion> FindByPrestamo(int prestamoId);
}
