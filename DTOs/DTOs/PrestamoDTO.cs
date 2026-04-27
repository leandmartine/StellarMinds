using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.DTOs
{
        public class PrestamoDTO
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoPrestamo Estado { get; set; }
        public Usuario Socio { get; set; }
        public Montura Montura { get; set; }
        public Telescopio Telescopio { get; set; }
    }
}
