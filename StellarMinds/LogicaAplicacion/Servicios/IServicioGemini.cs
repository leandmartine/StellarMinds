using DTOs.DTOs;
using StellarMinds.Entidades.Enums;

namespace LogicaAplicacion.Servicios;

// Resultado retornado por el servicio de evaluación de Gemini (RF07).
// Indicador: IDEAL | ADECUADO | NO_RECOMENDABLE (definido en el enum IndicadorAdecuacion).
// Detalle: explicación breve en español (máx. 2 oraciones; vacío si el indicador es IDEAL).
public record ResultadoGemini(IndicadorAdecuacion Indicador, string Detalle);

// Contrato del servicio de evaluación astronómica con Gemini AI (RF07).
// permite que el caso de uso AltaObservacion dependa de la abstracción y no de la implementación concreta.
// Implementado por ServicioGemini en la capa WebAPI.
public interface IServicioGemini
{
    // Evalúa de forma asíncrona si el equipo de un préstamo es adecuado
    // para observar el objeto celeste indicado, consultando la API de Google Gemini.
    // DTO del préstamo activo del socio. Contiene los nombres del telescopio, montura,
    // cámara (si aplica) y ocular (si aplica) que se envían al prompt de Gemini.
    // DTO del objeto celeste a observar (Nombre, Tipo, Magnitud).
    // Se incluye en el prompt para que Gemini evalúe la combinación equipo-objeto.
    // Retorna:
    //   - Indicador: IDEAL, ADECUADO o NO_RECOMENDABLE
    //   - Detalle: motivo si no es IDEAL (máx. 300 caracteres, según letra RF07)
    // Lanza Exception si Gemini no responde o la respuesta no puede interpretarse.
    Task<ResultadoGemini> EvaluarAsync(PrestamoDTO prestamo, ObjetoCelesteDTO objeto);
}
