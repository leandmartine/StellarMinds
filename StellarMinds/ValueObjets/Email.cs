using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class Email : IValidable
{
    public string mail { get; private set; }

    public Email(string mail)
    {
        this.mail = mail;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(mail))
            throw new InvalidEmailException("El email no puede estar vacio");
    }
}