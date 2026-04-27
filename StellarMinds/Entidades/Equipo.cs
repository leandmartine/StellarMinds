using StellarMinds.Excepciones;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.Entidades
{
    public abstract class Equipo : IValidable
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Stock { get; set; }

        // Constructor vacio requerido por EF Core y por el model binding de MVC.
        protected Equipo() { }

        protected Equipo(string marca, string modelo, int stock)
        {
            this.Marca = marca;
            this.Modelo = modelo;
            this.Stock = stock;
        }

        // virtual para que las subclases extiendan y primero validen lo comun.
        public virtual void Validar()
        {
            if (string.IsNullOrWhiteSpace(this.Marca))
                throw new EquipoException("La marca es requerida");

            if (string.IsNullOrWhiteSpace(this.Modelo))
                throw new EquipoException("El modelo es requerido");

            // Regla de la letra: las magnitudes son positivas o cero.
            if (this.Stock < 0)
                throw new EquipoException("El stock no puede ser negativo");
        }
    }
}
