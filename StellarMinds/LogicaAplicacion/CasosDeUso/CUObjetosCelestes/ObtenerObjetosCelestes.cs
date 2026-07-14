using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUObjetosCelestes;

public class ObtenerObjetosCelestes : IObtenerObjetosCelestes
{
    private readonly IRepositorioObjetoCeleste _repoObjeto;

    public ObtenerObjetosCelestes(IRepositorioObjetoCeleste repoObjeto)
    {
        _repoObjeto = repoObjeto;
    }

    public IEnumerable<ObjetoCelesteDTO> ObtenerTodos() =>
        _repoObjeto.FindAll().Select(o => new ObjetoCelesteDTO
        {
            Id = o.Id,
            Nombre = o.Nombre,
            Tipo = o.Tipo,
            Magnitud = o.Magnitud
        });
}
