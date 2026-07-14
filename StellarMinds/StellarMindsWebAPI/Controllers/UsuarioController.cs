using DTOs;
using DTOs.DTOs;
using DTOs.Mappers;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;
using StellarMinds.ValueObjets;

namespace StellarMindsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IAltaUsuario _altaUsuario;
    private readonly IObtenerUsuarios _obtenerUsuarios;
    private readonly IObtenerUsuarioPorId _obtenerUsuarioPorId;
    private readonly ILoginUsuario _loginUsuario;
    private readonly ILogOutUsuario _logOutUsuario;

    public UsuarioController(
        IAltaUsuario altaCU,
        IObtenerUsuarios obtenerTodosCU,
        IObtenerUsuarioPorId obtenerPorIdCU,
        ILoginUsuario loginCU,
        ILogOutUsuario logOutCU)
    {
        _altaUsuario = altaCU;
        _obtenerUsuarios = obtenerTodosCU;
        _obtenerUsuarioPorId = obtenerPorIdCU;
        _loginUsuario = loginCU;
        _logOutUsuario = logOutCU;
    }

    // GET api/Usuario
    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR,COORDINADOR")]
    public IActionResult Get()
    {
        try
        {
            IEnumerable<UsuarioDTO> usuarios = _obtenerUsuarios.ObtenerUsuarios();
            IEnumerable<UsuarioResumenDTO> resultado = usuarios.Select(u => new UsuarioResumenDTO
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto?.NombreCompletoTexto,
                NombreUsuario = u.NombreUsuario,
                Telefono = u.Telefono,
                Rol = u.rol
            });
            return Ok(resultado);
        }
        catch (UsuarioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/Usuario/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public ActionResult<UsuarioDTO> GetById(int id)
    {
        try
        {
            UsuarioDTO usuario = _obtenerUsuarioPorId.BuscarUsuarioPorId(id);
            return Ok(usuario);
        }
        catch (UsuarioException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/Usuario
    [HttpPost]
    [Authorize(Roles = "ADMINISTRADOR")]
    public ActionResult<UsuarioDTO> Post([FromBody] UsuarioAltaDTO dto)
    {
        try
        {
            UsuarioDTO usuarioDto = new UsuarioDTO
            {
                NombreCompleto = new NombreCompleto(dto.NombreCompleto),
                Direccion = new Direccion(dto.Calle, dto.Numero, dto.Apartamento, dto.Esquina, dto.Departamento, dto.Pais),
                Telefono = dto.Telefono,
                Mail = new Email(dto.Mail),
                NombreUsuario = dto.NombreUsuario,
                Contrasena = new Contrasenha(dto.Contrasena),
                rol = dto.Rol
            };

            _altaUsuario.AltaUsuario(usuarioDto);
            return Created($"api/Usuario", null);
        }
        catch (UsuarioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST api/Usuario/login
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<UsuarioSesionDTO> Login([FromBody] UsuarioLoginDTO dto)
    {
        try
        {
            UsuarioDTO usr = _loginUsuario.LoginUsuario(dto);
            string token = JWTHandler.JWTHandler.GenerarToken(usr);
            return Ok(new
            {
                Token = token,
                Usuario = usr
            });
        }
        catch (Exception)
        {
            return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });
        }
    }

    // POST api/Usuario/logout/{id}
    [HttpPost("logout/{id}")]
    public IActionResult Logout(int id)
    {
        try
        {
            _logOutUsuario.LogOut(id);
        }
        catch (UsuarioException)
        {
            // Si el usuario ya no existe igual completamos el logout
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        return Ok();
    }
}
