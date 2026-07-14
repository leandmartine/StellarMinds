using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StellarMindsMVC.Models;

namespace StellarMindsMVC.Filtros;

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
        ISession session = context.HttpContext.Session;

        string? nombreUsuario = session.GetString("NombreUsuario");
        if (string.IsNullOrEmpty(nombreUsuario))
        {
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
            return;
        }

        if (_rolesPermitidos.Length == 0) return;

        string? rolStr = session.GetString("Rol");
        if (string.IsNullOrEmpty(rolStr) || !Enum.TryParse<RolDeUsuario>(rolStr, out RolDeUsuario rol))
        {
            session.Clear();
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
            return;
        }

        if (!_rolesPermitidos.Contains(rol))
            context.Result = new RedirectToActionResult("Login", "Usuario", null);
    }
}
