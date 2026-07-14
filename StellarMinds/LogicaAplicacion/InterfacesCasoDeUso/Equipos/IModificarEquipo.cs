using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Equipos;

public interface IModificarEquipo
{
    // El id viene de la ruta (PUT api/Equipo/{tipo}/{id}); el DTO trae solo los datos del equipo.
    void Modificar(int id, EquipoAltaDTO aModificar);
}
