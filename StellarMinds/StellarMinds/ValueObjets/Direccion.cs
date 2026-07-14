using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class Direccion : IValidable
{
    public string Calle { get; private set; }
    public int Numero { get; private set; }
    public int Apartamento { get; private set; }
    public string Esquina { get; private set; }
    public string Departamento { get; private set; }
    public string Pais { get; private set; }
    
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