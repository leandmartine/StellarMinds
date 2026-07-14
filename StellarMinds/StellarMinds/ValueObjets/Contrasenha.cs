using Microsoft.EntityFrameworkCore;
using StellarMinds.Excepciones.ExcepcionesVO;
using StellarMinds.InterfacesDominio;

namespace StellarMinds.ValueObjets;

// Value Object de la contraseña. Las reglas de complejidad se aplican en Validar().
[Owned]
public class Contrasenha : IValidable
{
    public string Pass { get; private set; }
    public Contrasenha(string pass)
    {
        this.Pass = pass;
    }

    public void Validar()
    {
        const string caracteresEspeciales = "!@#$%^&*";

        if (string.IsNullOrEmpty(Pass))
            throw new InvalidContrasenhaException("La contrasena no puede estar vacia.");

        if (Pass.Length < 8)
            throw new InvalidContrasenhaException("La contrasena debe tener al menos 8 caracteres.");

        if (!Pass.Any(char.IsUpper))
            throw new InvalidContrasenhaException("La contrasena debe tener al menos una mayuscula.");

        if (!Pass.Any(char.IsLower))
            throw new InvalidContrasenhaException("La contrasena debe tener al menos una minuscula.");

        if (!Pass.Any(char.IsDigit))
            throw new InvalidContrasenhaException("La contrasena debe tener al menos un numero.");

        if (!Pass.Any(c => caracteresEspeciales.Contains(c)))
            throw new InvalidContrasenhaException("La contrasena debe tener al menos un caracter especial (!@#$%^&*).");
    }
}
