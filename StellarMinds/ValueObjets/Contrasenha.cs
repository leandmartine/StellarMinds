using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class Contrasenha : IValidable
{
    public string contrasenha { get; private set; }

    public Contrasenha(string contrasenha)
    {
        this.contrasenha = contrasenha;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(contrasenha))
            throw new InvalidContrasenhaException("La contraseña no puede estar vacia");
    }
}