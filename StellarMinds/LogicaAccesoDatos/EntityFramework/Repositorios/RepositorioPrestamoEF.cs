using Microsoft.EntityFrameworkCore;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioPrestamoEF : IRepositorioPrestamo
{
    private readonly StellarMindsContext _context;

    public RepositorioPrestamoEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Alta(Prestamo aAgregar)
    {
        _context.Prestamos.Add(aAgregar);
        _context.SaveChanges();
    }

    public void Modificar(Prestamo aModificar)
    {
        Prestamo existente = _context.Prestamos.FirstOrDefault(p => p.Id == aModificar.Id);
        if (existente == null)
            throw new PrestamoException("El préstamo a modificar no existe");

        existente.Estado = aModificar.Estado;
        existente.FechaInicio = aModificar.FechaInicio;
        existente.FechaFin = aModificar.FechaFin;
        _context.SaveChanges();
    }

    public void Baja(int id)
    {
        Prestamo existente = _context.Prestamos.FirstOrDefault(p => p.Id == id);
        if (existente == null)
            throw new PrestamoException("El préstamo no existe");

        _context.Prestamos.Remove(existente);
        _context.SaveChanges();
    }

    public IEnumerable<Prestamo> FindAll()
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .ToList();
    }

    public Prestamo FindById(int id)
    {
        Prestamo encontrado = _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .FirstOrDefault(p => p.Id == id);

        if (encontrado == null)
            throw new PrestamoException("Préstamo no encontrado");
        return encontrado;
    }

    public IEnumerable<Prestamo> FindBySocio(int socioId)
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .Where(p => p.Socio.Id == socioId)
            .ToList();
    }

    public IEnumerable<Prestamo> FindActivosBySocio(int socioId)
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .Where(p => p.Socio.Id == socioId && p.Estado == EstadoPrestamo.PRESTAMO)
            .ToList();
    }

    public IEnumerable<Prestamo> FindBySocioAndPeriod(int socioId, int mes, int anio)
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .Where(p => p.Socio.Id == socioId
                     && p.FechaInicio.Month == mes
                     && p.FechaInicio.Year == anio)
            .ToList();
    }

    public IEnumerable<Prestamo> FindByTelescopio(int telescopioId)
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .Where(p => p.Telescopio.Id == telescopioId)
            .ToList();
    }

    // Filtra en BD todos los préstamos que referencien el equipo dado en cualquiera de sus roles
    // (telescopio, montura, cámara u ocular). Cámara y Ocular son opcionales, de ahí la guarda != null.
    public IEnumerable<Prestamo> FindByEquipo(int equipoId)
    {
        return _context.Prestamos
            .Include(p => p.Socio)
            .Include(p => p.Telescopio)
            .Include(p => p.Montura)
            .Include(p => p.Camara)
            .Include(p => p.Ocular)
            .Where(p => p.Telescopio.Id == equipoId ||
                        p.Montura.Id == equipoId ||
                        (p.Camara.Id != null && p.Camara.Id == equipoId) ||
                        (p.Ocular.Id != null && p.Ocular.Id == equipoId))
            .ToList();
    }
}
