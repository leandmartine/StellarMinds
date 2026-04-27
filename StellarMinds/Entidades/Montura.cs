using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;

namespace StellarMinds.Entidades
{
    public class Montura : Equipo
    {
        public TipoMontura Tipo { get; set; }
        public decimal CargaSoportada { get; set; }
        public bool Computorizado { get; set; }

        public Montura() : base() { }

        public Montura(string marca, string modelo, int stock,
                       TipoMontura tipo, decimal cargaSoportada, bool computorizado)
            : base(marca, modelo, stock)
        {
            this.Tipo = tipo;
            this.CargaSoportada = cargaSoportada;
            this.Computorizado = computorizado;
        }

        public override void Validar()
        {
            base.Validar();

            // Defensivo: si alguien manda un valor fuera del rango del enum.
            if (!System.Enum.IsDefined(typeof(TipoMontura), this.Tipo))
                throw new EquipoException("El tipo de montura no es valido");

            if (this.CargaSoportada < 0)
                throw new EquipoException("La carga soportada no puede ser negativa");
        }
    }
}
