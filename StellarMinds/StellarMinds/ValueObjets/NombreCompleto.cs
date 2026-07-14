using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class NombreCompleto : IValidable
{
    public string NombreCompletoTexto { get; private set; }
    private NombreCompleto() { }
    public NombreCompleto(string nombreCompleto)
    {
        this.NombreCompletoTexto = nombreCompleto;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(NombreCompletoTexto))
            throw new InvalidNombreCompletoException("El nombre no puede estar vacio");
    }
}