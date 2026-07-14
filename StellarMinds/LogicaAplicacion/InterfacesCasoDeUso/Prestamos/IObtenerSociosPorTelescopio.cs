using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.Prestamos;

public interface IObtenerSociosPorTelescopio
{
    IEnumerable<UsuarioResumenDTO> Obtener(int telescopioId);
}
