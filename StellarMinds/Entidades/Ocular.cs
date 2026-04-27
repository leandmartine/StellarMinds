using StellarMinds.Excepciones;

namespace StellarMinds.Entidades
{
    public class Ocular : Equipo
    {
        public decimal Diametro { get; set; }
        public decimal Angulo { get; set; }

        public Ocular() : base() { }

        public Ocular(string marca, string modelo, int stock,
                      decimal diametro, decimal angulo)
            : base(marca, modelo, stock)
        {
            this.Diametro = diametro;
            this.Angulo = angulo;
        }

        public override void Validar()
        {
            base.Validar();

            if (this.Diametro < 0)
                throw new EquipoException("El diametro no puede ser negativo");

            if (this.Angulo < 0)
                throw new EquipoException("El angulo de vision no puede ser negativo");
        }
    }
}
