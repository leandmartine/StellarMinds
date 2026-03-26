using StellarMinds.Enums;
using StellarMinds.ValueObjets;

namespace StellarMinds.Entidades;

public class Usuario
{
    public int Id {get; set;}
    public NombreCompleto NombreCompleto {get; set;}
    public Direccion Direccion {get; set;}
    public string Telefono {get; set;}
    public Email Mail {get; set;}
    public string NombreUsuario {get; set;}
    public Contrasenha Contrasena {get; set;}
    public RolDeUsuario Rol {get; set;}
    

    public Usuario(string unNombreCompleto, string calle, int numero, int apartamento, string esquina, string departamento, string pais, string telefono, string unMail, string nombreUsuario, string unaContrasena, RolDeUsuario rol)
    {
        this.NombreCompleto = new NombreCompleto(unNombreCompleto);
        this.Mail = new Email(unMail);
        this.Direccion = new Direccion( calle, numero, apartamento, esquina, departamento, pais);
        this.Contrasena = new Contrasenha(unaContrasena);
        this.Rol = new RolDeUsuario();
    }

    public Usuario(){ }

    public void Validar()
    {
        this.NombreCompleto.Validar();
        this.Mail.Validar();
        this.Direccion.Validar();
        this.Contrasena.Validar();
    }
    
}