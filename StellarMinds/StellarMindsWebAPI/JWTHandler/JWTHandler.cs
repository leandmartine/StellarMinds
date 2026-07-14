using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DTOs;

namespace StellarMindsWebAPI.JWTHandler;

public class JWTHandler
{
    public static string GenerarToken(UsuarioDTO usuarioDto)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();


        byte[] clave = Encoding.ASCII.GetBytes("ZWRpw6fDo28gZW0gY29tcHV0YWRvcmE=");


        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuarioDto.NombreUsuario),
                new Claim(ClaimTypes.Role, usuarioDto.rol.ToString())
            }),
            Expires = DateTime.UtcNow.AddMonths(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(clave),
            SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
