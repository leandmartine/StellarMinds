using DTOs.DTOs;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;

namespace DTOs.Mappers;

public static class EquipoDTOMapper
{
    public const string TELESCOPIO = "TELESCOPIO";
    public const string MONTURA = "MONTURA";
    public const string CAMARA = "CAMARA";
    public const string OCULAR = "OCULAR";

    public static EquipoDTO ToDto(Equipo equipo)
    {
        if (equipo == null) return null;

        EquipoDTO dto = new EquipoDTO
        {
            Id = equipo.Id,
            Marca = equipo.Marca,
            Modelo = equipo.Modelo,
            Stock = equipo.Stock
        };

        // Segun la clase concreta, marcamos el discriminador y copiamos campos.
        // Usamos switch por tipo para mantenerlo simple.
        switch (equipo)
        {
            case Telescopio t:
                dto.TipoEquipo = TELESCOPIO;
                dto.Apertura = t.Apertura;
                dto.RelFocal = t.RelFocal;
                dto.DistanciaFocal = t.DistanciaFocal;
                dto.Peso = t.Peso;
                break;

            case Montura m:
                dto.TipoEquipo = MONTURA;
                dto.TipoMontura = m.Tipo;
                dto.CargaSoportada = m.CargaSoportada;
                dto.Computorizado = m.Computorizado;
                break;

            case Camara c:
                dto.TipoEquipo = CAMARA;
                dto.Sensor = c.Sensor;
                dto.Resolucion = c.Resolucion;
                dto.Pixel = c.Pixel;
                break;

            case Ocular o:
                dto.TipoEquipo = OCULAR;
                dto.Diametro = o.Diametro;
                dto.Angulo = o.Angulo;
                break;

            default:
                throw new EquipoException("Tipo de equipo desconocido");
        }

        return dto;
    }

    // Convierte un DTO de alta/modificación (concreto por subtipo) a la entidad de dominio.
    // El switch es polimórfico sobre el tipo en tiempo de ejecución del DTO: como cada subtipo
    // trae solo sus campos no anulables, no hace falta el coalescing (?? 0) de antes.
    // El Id NO se asigna acá: en alta lo genera la BD y en modificación lo fija el caso de uso desde la ruta.
    public static Equipo FromAltaDto(EquipoAltaDTO dto)
    {
        if (dto == null)
            throw new EquipoException("Debe especificarse un equipo");

        switch (dto)
        {
            case TelescopioAltaDTO t:
                return new Telescopio
                {
                    Marca = t.Marca,
                    Modelo = t.Modelo,
                    Stock = t.Stock,
                    Apertura = t.Apertura,
                    RelFocal = t.RelFocal,
                    DistanciaFocal = t.DistanciaFocal,
                    Peso = t.Peso
                };

            case MonturaAltaDTO m:
                return new Montura
                {
                    Marca = m.Marca,
                    Modelo = m.Modelo,
                    Stock = m.Stock,
                    Tipo = m.TipoMontura,
                    CargaSoportada = m.CargaSoportada,
                    Computorizado = m.Computorizado
                };

            case CamaraAltaDTO c:
                return new Camara
                {
                    Marca = c.Marca,
                    Modelo = c.Modelo,
                    Stock = c.Stock,
                    Sensor = c.Sensor,
                    Resolucion = c.Resolucion,
                    Pixel = c.Pixel
                };

            case OcularAltaDTO o:
                return new Ocular
                {
                    Marca = o.Marca,
                    Modelo = o.Modelo,
                    Stock = o.Stock,
                    Diametro = o.Diametro,
                    Angulo = o.Angulo
                };

            default:
                throw new EquipoException("Tipo de equipo desconocido");
        }
    }
}
