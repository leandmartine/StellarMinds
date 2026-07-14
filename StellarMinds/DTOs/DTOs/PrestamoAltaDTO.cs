namespace DTOs.DTOs;

public class PrestamoAltaDTO
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int SocioId { get; set; }
    public int CoordinadorId { get; set; }
    public int TelescopioId { get; set; }
    public int MonturaId { get; set; }
    public int? CamaraId { get; set; }
    public int? OcularId { get; set; }
}
