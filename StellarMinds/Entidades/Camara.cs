using StellarMinds.Excepciones;

namespace StellarMinds.Entidades
{
    public class Camara : Equipo
    {
        public string Sensor { get; set; }
        public string Resolucion { get; set; }
        public decimal Pixel { get; set; }

        public Camara() : base() { }

        public Camara(string marca, string modelo, int stock,
                      string sensor, string resolucion, decimal pixel)
            : base(marca, modelo, stock)
        {
            this.Sensor = sensor;
            this.Resolucion = resolucion;
            this.Pixel = pixel;
        }

        public override void Validar()
        {
            base.Validar();

            if (string.IsNullOrWhiteSpace(this.Sensor))
                throw new EquipoException("El tipo de sensor es requerido (CMOS o CCD)");

            if (string.IsNullOrWhiteSpace(this.Resolucion))
                throw new EquipoException("La resolucion es requerida (ej: 3840x2160)");

            if (this.Pixel < 0)
                throw new EquipoException("El tamano del pixel no puede ser negativo");
        }
    }
}
