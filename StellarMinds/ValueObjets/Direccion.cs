using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;

namespace StellarMinds.ValueObjets;

[Owned]
public class Direccion
{
    public string Calle { get; set; }
    public int Numero { get; set; }
    public int Apartamento { get; set; }
    public string Esquina { get; set; }
    public string Departamento { get; set; }
    public string Pais { get; set; }
    
    public Direccion(string calle, int numero, int apartamento, string esquina, string departamento, string pais)
    {
        this.Calle = calle;
        this.Numero = numero;
        this.Apartamento = apartamento;
        this.Esquina = esquina;
        this.Departamento = departamento;
        this.Pais = pais;
    }
    

    public void Validar()
    {
        if (string.IsNullOrEmpty(Calle))
            throw new InvalidDireccionException("La calle no puede estar vacia");
        
        if (Numero == null || Numero < 1)
            throw new InvalidDireccionException("El numero no puede estar vacio");
        
        if (Apartamento == null)
            throw new InvalidDireccionException("El numero de apartamento no puede estar vacio");
        
        if (string.IsNullOrEmpty(Esquina))
            throw new InvalidDireccionException("La esquina no puede estar vacia");
        
        if (string.IsNullOrEmpty(Pais))
            throw new InvalidDireccionException("El pais no puede estar vacio");
    }
    
}