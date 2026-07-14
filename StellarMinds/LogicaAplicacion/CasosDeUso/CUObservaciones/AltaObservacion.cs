using DTOs.DTOs;
using LogicaAplicacion.InterfacesCasoDeUso.Observaciones;
using LogicaAplicacion.Servicios;
using StellarMinds.Entidades;
using StellarMinds.Entidades.Enums;
using StellarMinds.Excepciones;
using StellarMinds.InterfacesRepositorios;

namespace LogicaAplicacion.CasosDeUso.CUObservaciones;

public class AltaObservacion : IAltaObservacion
{
    private readonly IRepositorioObservacion _repoObservacion;
    private readonly IRepositorioPrestamo _repoPrestamo;
    private readonly IRepositorioObjetoCeleste _repoObjeto;
    private readonly IRepositorioUsuario _repoUsuario;
    private readonly IServicioGemini _gemini;

    public AltaObservacion(
        IRepositorioObservacion repoObservacion,
        IRepositorioPrestamo repoPrestamo,
        IRepositorioObjetoCeleste repoObjeto,
        IRepositorioUsuario repoUsuario,
        IServicioGemini gemini)
    {
        _repoObservacion = repoObservacion;
        _repoPrestamo = repoPrestamo;
        _repoObjeto = repoObjeto;
        _repoUsuario = repoUsuario;
        _gemini = gemini;
    }

    public ObservacionDTO Alta(ObservacionAltaDTO dto)
    {
        // Validaciones: el préstamo debe estar activo, pertenecer al socio y la fecha dentro del período
        Prestamo prestamo = _repoPrestamo.FindById(dto.PrestamoId);
        if (prestamo.Estado != EstadoPrestamo.PRESTAMO)
            throw new ObservacionException("Solo se pueden registrar observaciones en préstamos activos");
        if (prestamo.Socio.Id != dto.SocioId)
            throw new ObservacionException("El préstamo no pertenece al socio indicado");
        if (dto.Fecha < prestamo.FechaInicio || dto.Fecha > prestamo.FechaFin)
            throw new ObservacionException("La fecha de observación debe estar dentro del período del préstamo");

        // Carga las entidades necesarias para construir la observación y el DTO de retorno
        ObjetoCeleste objeto = _repoObjeto.FindById(dto.ObjetoId);
        Usuario socio = _repoUsuario.FindById(dto.SocioId);

        // Resolución del Indicador y el Detalle de la observación.
        IndicadorAdecuacion indicador;
        string detalle;
        string? advertencia = null;

        if (!string.IsNullOrWhiteSpace(dto.Indicador))
        {
            // Si el socio ya evaluó en el front, reutilizamos ese resultado y no volvemos a llamar a Gemini.
            if (!Enum.TryParse(dto.Indicador, out indicador))
                indicador = IndicadorAdecuacion.ADECUADO;
            detalle = dto.Detalle;
        }
        else
        {
            // Sin evaluación previa: consultamos a Gemini. Si falla, igual registramos la observación.
            try
            {
                PrestamoDTO prestamoDto = CasosDeUso.CUPrestamos.ObtenerPrestamo.MapToDto(prestamo);
                ObjetoCelesteDTO objetoDto = new ObjetoCelesteDTO
                {
                    Id = objeto.Id,
                    Nombre = objeto.Nombre,
                    Tipo = objeto.Tipo,
                    Magnitud = objeto.Magnitud
                };

                ResultadoGemini resultado = _gemini.EvaluarAsync(prestamoDto, objetoDto).GetAwaiter().GetResult();
                indicador = resultado.Indicador;
                detalle = resultado.Detalle;
            }
            catch (Exception ex)
            {
                indicador = IndicadorAdecuacion.ADECUADO;
                detalle = "No se pudo generar descripción";
                advertencia = $"La observación se registró, pero no se pudo generar la descripción automática: {ex.Message}";
            }
        }

        // Garantía de servidor: la entidad exige Detalle no vacío (p. ej. Gemini devuelve detalle vacío en IDEAL).
        if (string.IsNullOrWhiteSpace(detalle))
            detalle = "Sin observaciones adicionales.";

        // Construye la entidad con los datos resueltos y la persiste
        Observacion observacion = new Observacion
        {
            Fecha = dto.Fecha,
            Prestamo = prestamo,
            Objeto = objeto,
            Socio = socio,
            Indicador = indicador,
            Detalle = detalle,
            TipoObservacion = dto.TipoObservacion
        };

        _repoObservacion.Alta(observacion);

        // Retorna el DTO completo al controller con los datos persistidos.
        // Advertencia != null indica que se guardó pero la IA no pudo evaluar (el front lo muestra por ViewBag).
        return new ObservacionDTO
        {
            Id = observacion.Id,
            Fecha = observacion.Fecha,
            PrestamoId = prestamo.Id,
            ObjetoId = objeto.Id,
            ObjetoNombre = objeto.Nombre,
            SocioId = socio.Id,
            SocioNombre = socio.NombreUsuario,
            Indicador = indicador,
            Detalle = detalle,
            TipoObservacion = dto.TipoObservacion,
            Advertencia = advertencia
        };
    }
}
