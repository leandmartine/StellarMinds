using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAccesoDatos.RepositorioMemoria;

public class RepositorioEquipo : IRepositorioEquipo
{
    // Lista estatica: los datos persisten entre requests mientras la app vive.
    public static List<Equipo> Equipos = new List<Equipo>();

    public RepositorioEquipo()
    {
        this.GenerarEquipos();
    }

    public void Alta(Equipo aAgregar)
    {
        aAgregar.Validar();

        // Id autoincremental: se lo pone el repositorio (equivalente a un IDENTITY de BD).
        aAgregar.Id = Equipos.Count == 0 ? 1 : Equipos.Max(e => e.Id) + 1;
        Equipos.Add(aAgregar);
    }

    public void Modificar(Equipo aModificar)
    {
        aModificar.Validar();

        // Buscamos el equipo existente y actualizamos sus datos.
        // LINQ basico con FirstOrDefault.
        Equipo actual = Equipos.FirstOrDefault(e => e.Id == aModificar.Id);
        if (actual == null)
            throw new EquipoException("El equipo a modificar no existe");

        // Se reemplaza en la lista para respetar el tipo concreto (Telescopio, Montura, etc.)
        int indice = Equipos.IndexOf(actual);
        Equipos[indice] = aModificar;
    }

    public void Baja(int id)
    {
        Equipo actual = Equipos.FirstOrDefault(e => e.Id == id);
        if (actual == null)
            throw new EquipoException("El equipo a eliminar no existe");

        // Nota: la letra dice que no se puede dar de baja un equipo en prestamo.
        // Esa precondicion se chequea en el CU de Baja (ahi inyectamos el
        // repositorio de prestamos). Aca solo se hace la baja fisica.
        Equipos.Remove(actual);
    }

    public IEnumerable<Equipo> FindAll()
    {
        return Equipos;
    }

    public Equipo FindById(int id)
    {
        Equipo encontrado = Equipos.FirstOrDefault(e => e.Id == id);
        if (encontrado == null)
            throw new EquipoException("Equipo no encontrado");
        return encontrado;
    }

    private void GenerarEquipos()
    {
        if (Equipos.Count != 0) return;

        // Precarga de datos de prueba. La letra pide "al menos 10 registros de cada tabla"
        // para la entrega final; aca dejo 3 de cada tipo para poder testear RF03.
        List<Equipo> precarga = new List<Equipo>
        {
            // Telescopios
            new Telescopio("Celestron",  "NexStar 8SE",    2, 203,  "f/10", 2032, 12.25m),
            new Telescopio("Sky-Watcher", "Explorer 150P", 3, 150,  "f/5",  750,  5.6m),
            new Telescopio("Meade",       "LX90 ACF",      1, 254,  "f/10", 2500, 17.0m),

            // Monturas
            new Montura("Sky-Watcher", "EQ6-R Pro",   2, TipoMontura.ECUATORIAL, 20,  true),
            new Montura("Celestron",   "AVX",         1, TipoMontura.ECUATORIAL, 13.6m, true),
            new Montura("iOptron",     "AZ Mount Pro",2, TipoMontura.ALTAZIMUTAL, 15,  true),

            // Camaras
            new Camara("ZWO",    "ASI2600MC Pro", 1, "CMOS", "6248x4176", 3.76m),
            new Camara("QHY",    "QHY5L-II-M",    4, "CMOS", "1280x960",  3.75m),
            new Camara("Canon",  "EOS Ra",        2, "CMOS", "6720x4480", 5.36m),

            // Oculares
            new Ocular("Tele Vue",   "Delos 10mm",     3, 10, 72),
            new Ocular("Explore Sci","68 Degree 24mm", 2, 24, 68),
            new Ocular("Baader",     "Morpheus 9mm",   5, 9,  76),
        };

        foreach (Equipo e in precarga)
            this.Alta(e);
    }
}
