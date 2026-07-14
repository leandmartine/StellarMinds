using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

// RF11: consulta de auditorías de préstamos, con o sin filtro por coordinador.
public class ObtenerAuditoriaPrestamos : IObtenerAuditoriaPrestamos
{
    private readonly IRepositorioAuditoriaPrestamo _repoAuditoria;

    public ObtenerAuditoriaPrestamos(IRepositorioAuditoriaPrestamo repoAuditoria)
    {
        _repoAuditoria = repoAuditoria;
    }

    public IEnumerable<AuditoriaPrestamoDTO> ObtenerTodas() =>
        _repoAuditoria.FindAll().Select(MapToDto);

    public IEnumerable<AuditoriaPrestamoDTO> ObtenerPorCoordinador(int coordinadorId) =>
        _repoAuditoria.FindByCoordinador(coordinadorId).Select(MapToDto);

    private static AuditoriaPrestamoDTO MapToDto(AuditoriaPrestamo a) => new AuditoriaPrestamoDTO
    {
        Id = a.Id,
        Accion = a.Accion,
        Fecha = a.Fecha,
        CoordinadorId = a.Coordinador.Id,
        CoordinadorNombre = a.Coordinador.NombreUsuario,
        PrestamoId = a.Prestamo.Id
    };
}
