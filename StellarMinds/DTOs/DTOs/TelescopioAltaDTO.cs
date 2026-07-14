using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs;

public class TelescopioAltaDTO : EquipoAltaDTO
{
    [Range(0, double.MaxValue, ErrorMessage = "La apertura no puede ser negativa")]
    public decimal Apertura { get; set; }

    [Required(ErrorMessage = "La relacion focal es obligatoria")]
    public string RelFocal { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "La distancia focal no puede ser negativa")]
    public decimal DistanciaFocal { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El peso no puede ser negativo")]
    public decimal Peso { get; set; }
}
