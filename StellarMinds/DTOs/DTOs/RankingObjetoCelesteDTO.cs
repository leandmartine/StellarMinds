using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

// DTO de salida para el ranking de objetos celestes (RF10).
// Representa cuántas veces fue observado un objeto celeste por los socios del club.
public class RankingObjetoCelesteDTO
{
    public string Nombre { get; set; }

    public TipoObjetoCeleste Tipo { get; set; }

   // Total de observaciones registradas para este objeto.
   // Calculado agrupando la tabla Observaciones por ObjetoId.
   // Solo aparecen objetos con al menos 1 observación (RF10: los nunca observados se excluyen).
    public int CantidadObservaciones { get; set; }
}
