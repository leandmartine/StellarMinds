using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;

public interface IObtenerObjetosCelestes
{
    IEnumerable<ObjetoCelesteDTO> ObtenerTodos();
}
