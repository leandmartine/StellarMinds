using StellarMinds.Entidades;

namespace DTOs.Mappers;

public class UsuarioDTOMapper
{
    public static UsuarioDTO ToDto(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Direccion = usuario.Direccion,
            Telefono = usuario.Telefono,
            Mail = usuario.Mail,
            NombreUsuario = usuario.NombreUsuario,
            rol = usuario.Rol
        };
    }

    public static Usuario FromDto(UsuarioDTO dto)
    {
        return new Usuario
        {
            Id = dto.Id,
            NombreCompleto = dto.NombreCompleto,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Mail = dto.Mail,
            NombreUsuario = dto.NombreUsuario,
            Contrasena = dto.Contrasena,
            Rol = dto.rol
        };
    }
}