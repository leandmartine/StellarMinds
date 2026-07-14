using System.Text;
using System.Text.Json;
using DTOs.DTOs;
using LogicaAplicacion.Servicios;
using StellarMinds.Entidades.Enums;

namespace StellarMindsWebAPI.Servicios;

// Evalúa la adecuación equipo/objeto celeste contra la API REST de Gemini (RF07).
// La clave se lee de appsettings.json ("GeminiApiKey").
public class ServicioGemini : IServicioGemini
{
    private HttpClient _http;
    private string _apiKey;

    private static JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web);

    public ServicioGemini(IHttpClientFactory factory, IConfiguration config)
    {
        _http = factory.CreateClient();
        _apiKey = config["GeminiApiKey"] ?? throw new InvalidOperationException("GeminiApiKey no configurada");
    }

    public async Task<ResultadoGemini> EvaluarAsync(PrestamoDTO prestamo, ObjetoCelesteDTO objeto)
    {
        string prompt = ConstruirPrompt(prestamo, objeto);

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
        StringContent content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        HttpResponseMessage resp = await _http.PostAsync(url, content);

        if (!resp.IsSuccessStatusCode)
        {
            int codigo = (int)resp.StatusCode;
            string motivo;

            switch (codigo)
            {
                case 429:
                    motivo = "se alcanzó el límite de solicitudes a la IA (429)";
                    break;
                case 400:
                    motivo = "la solicitud a la IA fue rechazada (400)";
                    break;
                case 401:
                case 403:
                    motivo = "credenciales de la IA inválidas o sin permisos";
                    break;
                default:
                    motivo = $"el servicio de IA respondió con el código {codigo}";
                    break;
            }

            throw new Exception($"No se pudo generar la descripción: {motivo}.");
        }

        string rawJson = await resp.Content.ReadAsStringAsync();
        return ParsearRespuesta(rawJson);
    }

    // Arma el prompt con los datos técnicos del equipo y del objeto en el JSON definido por la letra.
    // Si el préstamo trae cámara se evalúa como astrofotografía; si trae ocular, como observación visual.
    private static string ConstruirPrompt(PrestamoDTO p, ObjetoCelesteDTO o)
    {
        var datos = new Dictionary<string, object?>
        {
            ["telescopio"] = new
            {
                apertura_mm = p.TelescopioApertura,
                focal_mm = p.TelescopioFocal,
                relacion_focal = p.TelescopioRelFocal
            }
        };

        if (p.CamaraId.HasValue)
            datos["camara"] = new
            {
                sensor = p.CamaraSensor,
                resolucion_px = p.CamaraResolucion,
                pixel_size_um = p.CamaraPixel
            };

        if (p.OcularId.HasValue)
            datos["ocular"] = new
            {
                diametro_mm = p.OcularDiametro,
                campo_grados = p.OcularCampo
            };

        datos["objeto_celeste"] = new
        {
            nombre = o.Nombre,
            tipo = o.Tipo.ToString(),
            magnitud_aparente = o.Magnitud
        };

        string datosJson = JsonSerializer.Serialize(datos, _opts);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Eres un experto en astronomía. Según los datos técnicos del equipo, evalúa si es adecuado para observar o fotografiar el objeto celeste.");
        sb.AppendLine("Responde ÚNICAMENTE con un JSON de dos campos: \"indicador\" (uno de: IDEAL, ADECUADO, NO_RECOMENDABLE) y \"detalle\" (motivo breve en español, máximo 300 caracteres; vacío si es IDEAL).");
        sb.AppendLine();
        sb.AppendLine(datosJson);
        return sb.ToString();
    }

    private static ResultadoGemini ParsearRespuesta(string rawJson)
    {
        try
        {
            // Navegar la estructura anidada de la respuesta de Gemini
            using JsonDocument doc = JsonDocument.Parse(rawJson);
            string texto = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";

            texto = texto.Trim();
            if (texto.StartsWith("```"))
            {
                int inicio = texto.IndexOf('{');
                int fin = texto.LastIndexOf('}');
                if (inicio >= 0 && fin > inicio)
                    texto = texto[inicio..(fin + 1)];
            }

            // Parsear el JSON interno con "indicador" y "detalle"
            using JsonDocument resultado = JsonDocument.Parse(texto);
            string indicadorStr = resultado.RootElement.GetProperty("indicador").GetString() ?? "ADECUADO";
            string detalle = resultado.RootElement.GetProperty("detalle").GetString() ?? "";

            // RF07: garantía de servidor — aunque Gemini ignore la instrucción, el detalle nunca supera 300 caracteres
            if (detalle.Length > 300)
                detalle = detalle[..300];

            // Convertir el string al enum; ADECUADO como fallback si Gemini envía algo inesperado
            if (!Enum.TryParse<IndicadorAdecuacion>(indicadorStr, out IndicadorAdecuacion indicador))
                indicador = IndicadorAdecuacion.ADECUADO;

            return new ResultadoGemini(indicador, detalle);
        }
        catch (Exception ex)
        {
            throw new Exception("No se pudo interpretar la respuesta del servicio de IA. No es posible registrar la observación.", ex);
        }
    }
}
