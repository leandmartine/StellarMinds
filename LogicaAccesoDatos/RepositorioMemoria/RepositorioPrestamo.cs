namespace LogicaAccesoDatos.RepositorioMemoria;

using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

// Repositorio en memoria para Prestamo. Usa lista estática igual que los otros
// repositorios del proyecto para mantener consistencia arquitectónica.
// Todas las consultas usan LINQ con sintaxis de método (letra p.7).
public class RepositorioPrestamo : IRepositorioPrestamo
{
    public static List<Prestamo> Prestamos = new List<Prestamo>();

    public void Alta(Prestamo aAgregar)
    {
        aAgregar.Id = Prestamos.Count == 0 ? 1 : Prestamos.Max(p => p.Id) + 1;
        Prestamos.Add(aAgregar);
    }

    public void Modificar(Prestamo aModificar)
    {
        Prestamo actual = Prestamos.FirstOrDefault(p => p.Id == aModificar.Id);
        if (actual == null)
            throw new PrestamoException("El préstamo a modificar no existe");

        int indice = Prestamos.IndexOf(actual);
        Prestamos[indice] = aModificar;
    }

    public void Baja(int id)
    {
        Prestamo actual = Prestamos.FirstOrDefault(p => p.Id == id);
        if (actual == null)
            throw new PrestamoException("El préstamo a eliminar no existe");
        Prestamos.Remove(actual);
    }

    public IEnumerable<Prestamo> FindAll()
    {
        return Prestamos;
    }

    public Prestamo FindById(int id)
    {
        Prestamo encontrado = Prestamos.FirstOrDefault(p => p.Id == id);
        if (encontrado == null)
            throw new PrestamoException("Préstamo no encontrado");
        return encontrado;
    }

    // RF05: busca préstamos de un socio con un estado específico.
    // LINQ con sintaxis de método: Where filtra, ToList materializa.
    public IEnumerable<Prestamo> FindByUsuarioYEstado(int socioId, EstadoPrestamo estado)
    {
        return Prestamos
            .Where(p => p.Socio.Id == socioId && p.Estado == estado)
            .ToList();
    }

    // RF03/RF04: verifica si un equipo (por Id) está en algún préstamo activo.
    // Se usa para impedir la baja de equipos prestados y para chequear disponibilidad.
    public bool EquipoEnPrestamo(int equipoId)
    {
        return Prestamos.Any(p =>
            p.Estado == EstadoPrestamo.PRESTAMO &&
            (p.Telescopio.Id == equipoId ||
             p.Montura.Id == equipoId ||
             (p.Camara != null && p.Camara.Id == equipoId) ||
             (p.Ocular != null && p.Ocular.Id == equipoId)));
    }
}
