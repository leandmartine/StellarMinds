using StellarMinds.Excepciones;

namespace StellarMinds.Entidades
{
    public class Telescopio : Equipo
    {
        public decimal Apertura { get; set; }
        public string RelFocal { get; set; }
        public decimal DistanciaFocal { get; set; }
        public decimal Peso { get; set; }

        public Telescopio() : base() { }

        public Telescopio(string marca, string modelo, int stock,
                          decimal apertura, string relFocal, decimal distanciaFocal, decimal peso)
            : base(marca, modelo, stock)
        {
            this.Apertura = apertura;
            this.RelFocal = relFocal;
            this.DistanciaFocal = distanciaFocal;
            this.Peso = peso;
        }

        public override void Validar()
        {
            base.Validar();

            if (this.Apertura < 0)
                throw new EquipoException("La apertura no puede ser negativa");

            if (string.IsNullOrWhiteSpace(this.RelFocal))
                throw new EquipoException("La relacion focal es requerida (ej: f/10)");

            if (this.DistanciaFocal < 0)
                throw new EquipoException("La distancia focal no puede ser negativa");

            if (this.Peso < 0)
                throw new EquipoException("El peso no puede ser negativo");
        }
    }
}
