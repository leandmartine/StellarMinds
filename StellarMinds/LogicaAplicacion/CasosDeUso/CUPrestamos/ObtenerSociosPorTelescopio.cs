using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

// RF09: socios que tomaron en préstamo un telescopio dado, sin repetir y ordenados por nombre desc.
public class ObtenerSociosPorTelescopio : IObtenerSociosPorTelescopio
{
    private readonly IRepositorioPrestamo _repoPrestamo;
    public ObtenerSociosPorTelescopio(IRepositorioPrestamo repoPrestamo)
    {
        _repoPrestamo = repoPrestamo;
    }

    public IEnumerable<UsuarioResumenDTO> Obtener(int telescopioId)
    {
        return _repoPrestamo
            .FindByTelescopio(telescopioId)
            .Select(p => p.Socio)
            .DistinctBy(u => u.Id)
            .OrderByDescending(u => u.NombreCompleto.NombreCompletoTexto)
            .Select(u => new UsuarioResumenDTO
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto.NombreCompletoTexto,
                NombreUsuario = u.NombreUsuario,
                Telefono = u.Telefono,
                Rol = u.Rol
            });
    }
}
