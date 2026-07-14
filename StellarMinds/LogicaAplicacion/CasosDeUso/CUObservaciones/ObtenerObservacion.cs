using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Observaciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUObservaciones;

public class ObtenerObservacion : IObtenerObservaciones
{
    private readonly IRepositorioObservacion _repoObservacion;

    public ObtenerObservacion(IRepositorioObservacion repoObservacion)
    {
        _repoObservacion = repoObservacion;
    }

    public IEnumerable<ObservacionDTO> ObtenerTodas() =>
        _repoObservacion.FindAll().Select(o => new ObservacionDTO
        {
            Id = o.Id,
            Fecha = o.Fecha,
            PrestamoId = o.Prestamo.Id,
            ObjetoId = o.Objeto.Id,
            ObjetoNombre = o.Objeto.Nombre,
            SocioId = o.Socio.Id,
            SocioNombre = o.Socio.NombreUsuario,
            Indicador = o.Indicador,
            Detalle = o.Detalle,
            TipoObservacion = o.TipoObservacion
        });
}
