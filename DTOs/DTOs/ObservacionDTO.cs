using StellarMinds.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.DTOs
{
    public class ObservacionDTO
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Prestamo Prestamo { get; set; }
        public ObjetoCeleste Objeto { get; set; }
        public Usuario Socio { get; set; }
    }
}
