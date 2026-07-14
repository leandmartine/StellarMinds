using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

[Owned]
public class Email : IValidable
{
    public string Mail { get; private set; }

    public Email(string mail)
    {
        this.Mail = mail;
    }

    public void Validar()
    {
        if (string.IsNullOrEmpty(Mail))
            throw new InvalidEmailException("El email no puede estar vacio");
    }
}