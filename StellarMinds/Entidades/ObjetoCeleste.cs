using StellarMinds.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.Entidades
{
    public class ObjetoCeleste
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoObjetoCeleste Tipo { get; set; }
        public decimal Magnitud { get; set; }
    }
}
