using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs;

public class CamaraAltaDTO : EquipoAltaDTO
{
    [Required(ErrorMessage = "El tipo de sensor es obligatorio (CMOS o CCD)")]
    public string Sensor { get; set; }

    [Required(ErrorMessage = "La resolucion es obligatoria")]
    public string Resolucion { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El tamano del pixel no puede ser negativo")]
    public decimal Pixel { get; set; }
}
