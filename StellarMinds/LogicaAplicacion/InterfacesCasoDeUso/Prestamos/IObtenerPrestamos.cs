using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

public interface IObtenerPrestamos
{
    IEnumerable<PrestamoDTO> ObtenerTodos();

    PrestamoDTO ObtenerPorId(int id);
}
