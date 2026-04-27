using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StellarMinds.Enums;

namespace StellarMindsWeb.Filtros;

// Filtro propio. Lo aplicamos como atributo sobre controllers o acciones
// concretas. Tiene dos usos:
//   [Autorizacion]                                  -> solo requiere estar logueado
//   [Autorizacion(RolDeUsuario.ADMINISTRADOR)]      -> ademas exige ese(s) rol(es)
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AutorizacionAttribute : Attribute, IAuthorizationFilter
{
    private readonly RolDeUsuario[] _rolesPermitidos;

    public AutorizacionAttribute(params RolDeUsuario[] rolesPermitidos)
    {
        _rolesPermitidos = rolesPermitidos ?? Array.Empty<RolDeUsuario>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;

        // Paso 1: chequeo basico de "esta logueado?".
        // Miramos la variable de sesion NombreUsuario: si no existe, lo mandamos al Login.
        string nombreUsuario = session.GetString("NombreUsuario");
        if (string.IsNullOrEmpty(nombreUsuario))
        {
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
            return;
        }

        // Paso 2: chequeo por rol (solo si el atributo se configuro con roles).
        // Guardamos el Id en sesion y buscamos el usuario para saber su Rol.
        if (_rolesPermitidos.Length == 0) return;

        int? idUsuario = session.GetInt32("IdUsuario");
        if (idUsuario == null)
        {
            // Sesion corrupta: tiene nombre pero no tiene Id. Forzamos re-login.
            session.Clear();
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
            return;
        }

        // Resolvemos el caso de uso por el contenedor de DI.
        var obtenerPorId = context.HttpContext.RequestServices
            .GetRequiredService<IObtenerUsuarioPorId>();

        try
        {
            var dto = obtenerPorId.BuscarUsuarioPorId(idUsuario.Value);

            if (!_rolesPermitidos.Contains(dto.rol))
            {
                // Logueado pero sin permiso para esta vista: lo mandamos al Login
                // segun lo que hablamos en el punto 2 (redirigir siempre al Login).
                context.Result = new RedirectToActionResult("Login", "Usuario", null);
                return;
            }
        }
        catch
        {
            // Si algo falla (ej: el usuario ya no existe), forzamos logout.
            session.Clear();
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
        }
    }
}
