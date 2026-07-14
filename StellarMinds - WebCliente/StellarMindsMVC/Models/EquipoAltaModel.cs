namespace StellarMindsMVC.Models;

// Base de los payloads de alta/modificación de equipo que se envían a la API.
// Espejo de EquipoAltaDTO: cada subtipo agrega solo sus campos, todos no anulables.
public abstract class EquipoAltaModel
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Stock { get; set; }
}
