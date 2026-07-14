using DTOs.DTOs;

namespace LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;

public interface IObtenerRankingObjetosCelestes
{
    IEnumerable<RankingObjetoCelesteDTO> ObtenerRanking();
}
