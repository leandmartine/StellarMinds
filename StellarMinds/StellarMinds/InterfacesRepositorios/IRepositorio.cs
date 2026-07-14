namespace StellarMinds.InterfacesRepositorios;

public interface IRepositorio<T> where T : class
{
    void Alta(T aAgregar);
    void Modificar(T aModificar);
    void Baja(int id);

    IEnumerable<T> FindAll();
    T FindById(int id);
}
