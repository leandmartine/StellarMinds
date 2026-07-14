using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioObjetoCelesteEF : IRepositorioObjetoCeleste
{
    private readonly StellarMindsContext _context;

    public RepositorioObjetoCelesteEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Alta(ObjetoCeleste aAgregar)
    {
        _context.ObjetosCelestes.Add(aAgregar);
        _context.SaveChanges();
    }

    public void Modificar(ObjetoCeleste aModificar)
    {
        ObjetoCeleste existente = _context.ObjetosCelestes.FirstOrDefault(o => o.Id == aModificar.Id);
        if (existente == null)
            throw new ObjetoCelesteException("El objeto celeste a modificar no existe");

        existente.Nombre = aModificar.Nombre;
        existente.Tipo = aModificar.Tipo;
        existente.Magnitud = aModificar.Magnitud;
        _context.SaveChanges();
    }

    public void Baja(int id)
    {
        ObjetoCeleste existente = _context.ObjetosCelestes.FirstOrDefault(o => o.Id == id);
        if (existente == null)
            throw new ObjetoCelesteException("El objeto celeste no existe");

        _context.ObjetosCelestes.Remove(existente);
        _context.SaveChanges();
    }

    public IEnumerable<ObjetoCeleste> FindAll()
    {
        return _context.ObjetosCelestes.ToList();
    }

    public ObjetoCeleste FindById(int id)
    {
        ObjetoCeleste encontrado = _context.ObjetosCelestes.FirstOrDefault(o => o.Id == id);
        if (encontrado == null)
            throw new ObjetoCelesteException("Objeto celeste no encontrado");
        return encontrado;
    }
}
