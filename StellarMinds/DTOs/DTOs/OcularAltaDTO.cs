using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs;

public class OcularAltaDTO : EquipoAltaDTO
{
    [Range(0, double.MaxValue, ErrorMessage = "El diametro no puede ser negativo")]
    public decimal Diametro { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El angulo no puede ser negativo")]
    public decimal Angulo { get; set; }
}
