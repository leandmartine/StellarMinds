using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

public class AltaPrestamo : IAltaPrestamo
{
    private readonly IRepositorioPrestamo _repoPrestamo;
    private readonly IRepositorioUsuario _repoUsuario;
    private readonly IRepositorioEquipo _repoEquipo;
    private readonly IRepositorioAuditoriaPrestamo _repoAuditoria;

    public AltaPrestamo(
        IRepositorioPrestamo repoPrestamo,
        IRepositorioUsuario repoUsuario,
        IRepositorioEquipo repoEquipo,
        IRepositorioAuditoriaPrestamo repoAuditoria)
    {
        _repoPrestamo = repoPrestamo;
        _repoUsuario = repoUsuario;
        _repoEquipo = repoEquipo;
        _repoAuditoria = repoAuditoria;
    }

    public void Alta(PrestamoAltaDTO dto)
    {
        Usuario socio = _repoUsuario.FindById(dto.SocioId);
        if (socio.Rol != StellarMinds.Enums.RolDeUsuario.SOCIO)
            throw new PrestamoException("Solo los socios pueden tener préstamos");

        Telescopio telescopio = _repoEquipo.FindById(dto.TelescopioId) as Telescopio
            ?? throw new PrestamoException("El equipo indicado no es un telescopio");

        Montura montura = _repoEquipo.FindById(dto.MonturaId) as Montura
            ?? throw new PrestamoException("El equipo indicado no es una montura");

        Camara? camara = null;
        if (dto.CamaraId.HasValue)
        {
            camara = _repoEquipo.FindById(dto.CamaraId.Value) as Camara
                ?? throw new PrestamoException("El equipo indicado no es una cámara");
        }

        Ocular? ocular = null;
        if (dto.OcularId.HasValue)
        {
            ocular = _repoEquipo.FindById(dto.OcularId.Value) as Ocular
                ?? throw new PrestamoException("El equipo indicado no es un ocular");
        }

        Prestamo prestamo = new Prestamo
        {
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Estado = EstadoPrestamo.PRESTAMO,
            Socio = socio,
            Telescopio = telescopio,
            Montura = montura,
            Camara = camara,
            Ocular = ocular
        };

        prestamo.Validar();

        // Descontamos stock
        telescopio.Stock--;
        montura.Stock--;
        if (camara != null) camara.Stock--;
        if (ocular != null) ocular.Stock--;

        _repoPrestamo.Alta(prestamo);

        Usuario coordinador = _repoUsuario.FindById(dto.CoordinadorId);
        AuditoriaPrestamo auditoria = new AuditoriaPrestamo
        {
            Accion = AccionAuditoria.ALTA_PRESTAMO,
            Fecha = DateTime.Now,
            Coordinador = coordinador,
            Prestamo = prestamo
        };
        _repoAuditoria.Registrar(auditoria);
    }
}
