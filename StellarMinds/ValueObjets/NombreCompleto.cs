using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class NombreCompleto : IValidable
{
    public string nombreCompleto { get; private set; }

    public NombreCompleto(string nombreCompleto)
    {
        this.nombreCompleto = nombreCompleto;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(nombreCompleto))
            throw new InvalidNombreCompletoException("El nombre no puede estar vacio");
    }
}