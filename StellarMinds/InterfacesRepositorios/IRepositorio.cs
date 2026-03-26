namespace StellarMinds.InterfacesRepositorios;

public interface IRepositorio<T> where T : class
{
    public void AltaUsuario(T aAgregar);
    public void ActualizarUsuario(T aModificar);
    public void BajaUsuario(int aEliminar);
    
    public IEnumerable<T> FindAll();
    public T FindById(int id);
}