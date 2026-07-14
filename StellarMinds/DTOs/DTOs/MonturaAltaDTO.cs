using System.ComponentModel.DataAnnotations;
using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs;

public class MonturaAltaDTO : EquipoAltaDTO
{
    public TipoMontura TipoMontura { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "La carga soportada no puede ser negativa")]
    public decimal CargaSoportada { get; set; }

    public bool Computorizado { get; set; }
}
