namespace LogicaAplicacion.CasosDeUso.CUPrestamos;

using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

// RF04: Alta de préstamo con todas las reglas de negocio de la letra.
// También dispara RF06 (auditoría) de forma automática al final del alta.
//
// Principio SRP: este caso de uso tiene una única responsabilidad — crear un préstamo válido.
// Principio DIP: depende de interfaces (IRepositorio*), no de implementaciones concretas.
public class AltaPrestamo : IAltaPrestamo
{
    private IRepositorioPrestamo _repoPrestamo;
    private IRepositorioEquipo _repoEquipo;
    private IRepositorioUsuario _repoUsuario;
    private IRepositorioAuditoriaPrestamo _repoAuditoria;

    // Inyección de dependencias por constructor: el contenedor de DI de ASP.NET
    // se encarga de construir e inyectar las implementaciones concretas.
    public AltaPrestamo(
        IRepositorioPrestamo repoPrestamo,
        IRepositorioEquipo repoEquipo,
        IRepositorioUsuario repoUsuario,
        IRepositorioAuditoriaPrestamo repoAuditoria)
    {
        _repoPrestamo = repoPrestamo;
        _repoEquipo = repoEquipo;
        _repoUsuario = repoUsuario;
        _repoAuditoria = repoAuditoria;
    }

    // coordinadorId: Id del usuario logueado en sesión que está creando el préstamo.
    // Se necesita para el registro de auditoría (RF06).
    public void Alta(PrestamoDTO dto, int coordinadorId)
    {
        // --- Validación de fechas ---
        if (dto.FechaInicio >= dto.FechaFin)
            throw new PrestamoException("La fecha de inicio debe ser anterior a la fecha de fin");

        // --- Cargar entidades desde repositorios ---
        // Los métodos FindById lanzan excepción si no existen (letra: validar existencia).
        Telescopio telescopio = (Telescopio)_repoEquipo.FindById(dto.TelescopioId);
        Montura montura = (Montura)_repoEquipo.FindById(dto.MonturaId);
        Usuario socio = _repoUsuario.FindById(dto.SocioId);
        Usuario coordinador = _repoUsuario.FindById(coordinadorId);

        Camara camara = null;
        Ocular ocular = null;

        if (dto.CamaraId != null)
            camara = (Camara)_repoEquipo.FindById(dto.CamaraId.Value);

        if (dto.OcularId != null)
            ocular = (Ocular)_repoEquipo.FindById(dto.OcularId.Value);

        // --- Regla: al menos cámara u ocular (letra p.3) ---
        if (camara == null && ocular == null)
            throw new PrestamoException("Debe solicitarse al menos una cámara o un ocular");

        // --- Regla: disponibilidad — stock > 0 (letra p.3) ---
        // El JS del cliente ya avisa, pero el servidor siempre debe validar también.
        if (telescopio.Stock == 0)
            throw new PrestamoException($"El telescopio '{telescopio.Marca} {telescopio.Modelo}' no tiene stock disponible");

        if (montura.Stock == 0)
            throw new PrestamoException($"La montura '{montura.Marca} {montura.Modelo}' no tiene stock disponible");

        if (camara != null && camara.Stock == 0)
            throw new PrestamoException($"La cámara '{camara.Marca} {camara.Modelo}' no tiene stock disponible");

        if (ocular != null && ocular.Stock == 0)
            throw new PrestamoException($"El ocular '{ocular.Marca} {ocular.Modelo}' no tiene stock disponible");

        // --- Regla: la montura debe soportar el peso del telescopio (letra p.3) ---
        if (montura.CargaSoportada < telescopio.Peso)
            throw new PrestamoException(
                $"La montura '{montura.Marca} {montura.Modelo}' soporta {montura.CargaSoportada}kg " +
                $"pero el telescopio pesa {telescopio.Peso}kg. Son incompatibles.");

        // --- Regla: si se presta cámara, la montura debe ser Ecuatorial o Híbrida (letra p.3) ---
        if (camara != null &&
            montura.Tipo != TipoMontura.ECUATORIAL &&
            montura.Tipo != TipoMontura.HIBRIDA)
        {
            throw new PrestamoException(
                "Para astrofotografía con cámara la montura debe ser Ecuatorial o Híbrida. " +
                $"La montura seleccionada es {montura.Tipo}.");
        }

        // --- Crear el préstamo ---
        Prestamo nuevoPrestamo = new Prestamo
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

        // --- Actualizar stock (letra p.3): al prestar, se descuenta la cantidad disponible ---
        telescopio.Stock--;
        montura.Stock--;
        if (camara != null) camara.Stock--;
        if (ocular != null) ocular.Stock--;

        // Actualizamos en el repositorio de equipos para que el cambio de stock persista.
        _repoEquipo.Modificar(telescopio);
        _repoEquipo.Modificar(montura);
        if (camara != null) _repoEquipo.Modificar(camara);
        if (ocular != null) _repoEquipo.Modificar(ocular);

        // Persistir el préstamo.
        _repoPrestamo.Alta(nuevoPrestamo);

        // --- RF06: registrar auditoría automáticamente ---
        // La letra dice "AUTOMÁTICO SISTEMA" — el usuario no hace nada, el sistema registra solo.
        AuditoriaPrestamo auditoria = new AuditoriaPrestamo
        {
            Accion = AccionAuditoria.ALTA_PRESTAMO,
            Fecha = DateTime.Now,
            Coordinador = coordinador,
            Prestamo = nuevoPrestamo
        };
        _repoAuditoria.Registrar(auditoria);
    }
}
