using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioObservacionEF : IRepositorioObservacion
{
    private readonly StellarMindsContext _context;

    public RepositorioObservacionEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Alta(Observacion aAgregar)
    {
        aAgregar.Validar();
        _context.Observaciones.Add(aAgregar);
        _context.SaveChanges();
    }

    public void Modificar(Observacion aModificar)
    {
        aModificar.Validar();
        Observacion existente = _context.Observaciones.FirstOrDefault(o => o.Id == aModificar.Id);
        if (existente == null)
            throw new ObservacionException("La observación a modificar no existe");

        existente.Fecha = aModificar.Fecha;
        existente.Indicador = aModificar.Indicador;
        existente.Detalle = aModificar.Detalle;
        existente.TipoObservacion = aModificar.TipoObservacion;
        _context.SaveChanges();
    }

    public void Baja(int id)
    {
        Observacion existente = _context.Observaciones.FirstOrDefault(o => o.Id == id);
        if (existente == null)
            throw new ObservacionException("La observación no existe");

        _context.Observaciones.Remove(existente);
        _context.SaveChanges();
    }

    public IEnumerable<Observacion> FindAll()
    {
        return _context.Observaciones
            .Include(o => o.Socio)
            .Include(o => o.Prestamo)
            .Include(o => o.Objeto)
            .ToList();
    }

    public Observacion FindById(int id)
    {
        Observacion encontrada = _context.Observaciones
            .Include(o => o.Socio)
            .Include(o => o.Prestamo)
            .Include(o => o.Objeto)
            .FirstOrDefault(o => o.Id == id);

        if (encontrada == null)
            throw new ObservacionException("Observación no encontrada");
        return encontrada;
    }

    public IEnumerable<Observacion> FindByPrestamo(int prestamoId)
    {
        return _context.Observaciones
            .Include(o => o.Socio)
            .Include(o => o.Prestamo)
            .Include(o => o.Objeto)
            .Where(o => o.Prestamo.Id == prestamoId)
            .ToList();
    }
}
