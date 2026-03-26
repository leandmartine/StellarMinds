using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;

namespace StellarMinds.ValueObjets;

[Owned]
public class NombreCompleto
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