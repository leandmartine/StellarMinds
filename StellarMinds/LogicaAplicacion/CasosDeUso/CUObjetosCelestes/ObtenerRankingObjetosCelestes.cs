using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUObjetosCelestes;

// RF10: agrupa las observaciones por objeto celeste y los ordena por cantidad descendente.
// Los objetos sin observaciones no aparecen porque parten de la tabla de observaciones.
public class ObtenerRankingObjetosCelestes : IObtenerRankingObjetosCelestes
{
    private IRepositorioObservacion _repoObservacion;

    public ObtenerRankingObjetosCelestes(IRepositorioObservacion repoObservacion)
    {
        _repoObservacion = repoObservacion;
    }

    public IEnumerable<RankingObjetoCelesteDTO> ObtenerRanking() =>
        _repoObservacion
            .FindAll()
            .GroupBy(o => o.Objeto.Id)
            .Select(g => new RankingObjetoCelesteDTO
            {
                Nombre = g.First().Objeto.Nombre,
                Tipo = g.First().Objeto.Tipo,
                CantidadObservaciones = g.Count()
            })
            .OrderByDescending(r => r.CantidadObservaciones);
}
