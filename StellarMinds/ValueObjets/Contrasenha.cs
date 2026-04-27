using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class Contrasenha : IValidable
{
    public string Pass { get; private set; }

    public Contrasenha(string contrasenha)
    {
        this.Pass = contrasenha;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(Pass))
            throw new InvalidContrasenhaException("La contraseña no puede estar vacia");
    }
}