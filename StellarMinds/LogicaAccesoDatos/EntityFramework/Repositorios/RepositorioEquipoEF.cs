using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.EntityFramework.Repositorios;

public class RepositorioEquipoEF : IRepositorioEquipo
{
    private readonly StellarMindsContext _context;

    public RepositorioEquipoEF(StellarMindsContext context)
    {
        _context = context;
    }

    public void Alta(Equipo aAgregar)
    {
        aAgregar.Validar();
        _context.Equipos.Add(aAgregar);
        _context.SaveChanges();
    }

    public void Modificar(Equipo aModificar)
    {
        aModificar.Validar();
        Equipo existente = _context.Equipos.FirstOrDefault(e => e.Id == aModificar.Id);
        if (existente == null)
            throw new EquipoException("El equipo a modificar no existe");

        existente.Marca = aModificar.Marca;
        existente.Modelo = aModificar.Modelo;
        existente.Stock = aModificar.Stock;

        if (existente is Telescopio t && aModificar is Telescopio tm)
        {
            t.Apertura = tm.Apertura;
            t.RelFocal = tm.RelFocal;
            t.DistanciaFocal = tm.DistanciaFocal;
            t.Peso = tm.Peso;
        }
        else if (existente is Montura mo && aModificar is Montura mom)
        {
            mo.Tipo = mom.Tipo;
            mo.CargaSoportada = mom.CargaSoportada;
            mo.Computorizado = mom.Computorizado;
        }
        else if (existente is Camara c && aModificar is Camara cm)
        {
            c.Sensor = cm.Sensor;
            c.Resolucion = cm.Resolucion;
            c.Pixel = cm.Pixel;
        }
        else if (existente is Ocular o && aModificar is Ocular om)
        {
            o.Diametro = om.Diametro;
            o.Angulo = om.Angulo;
        }

        _context.SaveChanges();
    }

    public void Baja(int id)
    {
        Equipo existente = _context.Equipos.FirstOrDefault(e => e.Id == id);
        if (existente == null)
            throw new EquipoException("El equipo a eliminar no existe");

        _context.Equipos.Remove(existente);
        _context.SaveChanges();
    }

    public IEnumerable<Equipo> FindAll()
    {
        return _context.Equipos.ToList();
    }

    public Equipo FindById(int id)
    {
        Equipo encontrado = _context.Equipos.FirstOrDefault(e => e.Id == id);
        if (encontrado == null)
            throw new EquipoException("Equipo no encontrado");
        return encontrado;
    }
}
