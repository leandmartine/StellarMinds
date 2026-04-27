using System.ComponentModel.DataAnnotations;
using StellarMinds.Entidades.Enums;

namespace DTOs.DTOs
{
    // DTO "aplanado": contiene los campos de las 4 clases concretas.
    // El string TipoEquipo actua como discriminador: el mapper lo usa para
    // saber que entidad concreta instanciar (Telescopio, Montura, Camara u Ocular).
    public class EquipoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        // Discriminador: "TELESCOPIO", "MONTURA", "CAMARA", "OCULAR".
        // Lo llenamos en los ViewModels y el mapper lo lee para decidir el tipo concreto.
        public string TipoEquipo { get; set; }

        // --- Campos propios de Telescopio ---
        [Range(0, double.MaxValue, ErrorMessage = "La apertura no puede ser negativa")]
        public decimal? Apertura { get; set; }
        public string RelFocal { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "La distancia focal no puede ser negativa")]
        public decimal? DistanciaFocal { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El peso no puede ser negativo")]
        public decimal? Peso { get; set; }

        // --- Campos propios de Montura ---
        public TipoMontura? TipoMontura { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "La carga soportada no puede ser negativa")]
        public decimal? CargaSoportada { get; set; }
        public bool? Computorizado { get; set; }

        // --- Campos propios de Camara ---
        public string Sensor { get; set; }
        public string Resolucion { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El tamano del pixel no puede ser negativo")]
        public decimal? Pixel { get; set; }

        // --- Campos propios de Ocular ---
        [Range(0, double.MaxValue, ErrorMessage = "El diametro no puede ser negativo")]
        public decimal? Diametro { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "El angulo no puede ser negativo")]
        public decimal? Angulo { get; set; }
    }
}
