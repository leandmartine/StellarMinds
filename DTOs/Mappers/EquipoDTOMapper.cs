using DTOs.DTOs;
using StellarMinds.Entidades;
using StellarMinds.Excepciones;

namespace DTOs.Mappers;

// Mapper polimorfico: como EquipoDTO es un DTO "aplanado", usamos el
// discriminador (TipoEquipo) para decidir la entidad concreta.
public static class EquipoDTOMapper
{
    public const string TELESCOPIO = "TELESCOPIO";
    public const string MONTURA    = "MONTURA";
    public const string CAMARA     = "CAMARA";
    public const string OCULAR     = "OCULAR";

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
        // Usamos pattern matching (switch por tipo) para mantenerlo simple.
        switch (equipo)
        {
            case Telescopio t:
                dto.TipoEquipo     = TELESCOPIO;
                dto.Apertura       = t.Apertura;
                dto.RelFocal       = t.RelFocal;
                dto.DistanciaFocal = t.DistanciaFocal;
                dto.Peso           = t.Peso;
                break;

            case Montura m:
                dto.TipoEquipo     = MONTURA;
                dto.TipoMontura    = m.Tipo;
                dto.CargaSoportada = m.CargaSoportada;
                dto.Computorizado  = m.Computorizado;
                break;

            case Camara c:
                dto.TipoEquipo = CAMARA;
                dto.Sensor     = c.Sensor;
                dto.Resolucion = c.Resolucion;
                dto.Pixel      = c.Pixel;
                break;

            case Ocular o:
                dto.TipoEquipo = OCULAR;
                dto.Diametro   = o.Diametro;
                dto.Angulo     = o.Angulo;
                break;

            default:
                throw new EquipoException("Tipo de equipo desconocido en ToDto");
        }

        return dto;
    }

    public static Equipo FromDto(EquipoDTO dto)
    {
        if (dto == null) return null;

        switch (dto.TipoEquipo)
        {
            case TELESCOPIO:
                return new Telescopio
                {
                    Id             = dto.Id,
                    Marca          = dto.Marca,
                    Modelo         = dto.Modelo,
                    Stock          = dto.Stock,
                    Apertura       = dto.Apertura       ?? 0,
                    RelFocal       = dto.RelFocal,
                    DistanciaFocal = dto.DistanciaFocal ?? 0,
                    Peso           = dto.Peso           ?? 0
                };

            case MONTURA:
                return new Montura
                {
                    Id             = dto.Id,
                    Marca          = dto.Marca,
                    Modelo         = dto.Modelo,
                    Stock          = dto.Stock,
                    Tipo           = dto.TipoMontura    ?? default,
                    CargaSoportada = dto.CargaSoportada ?? 0,
                    Computorizado  = dto.Computorizado  ?? false
                };

            case CAMARA:
                return new Camara
                {
                    Id         = dto.Id,
                    Marca      = dto.Marca,
                    Modelo     = dto.Modelo,
                    Stock      = dto.Stock,
                    Sensor     = dto.Sensor,
                    Resolucion = dto.Resolucion,
                    Pixel      = dto.Pixel ?? 0
                };

            case OCULAR:
                return new Ocular
                {
                    Id       = dto.Id,
                    Marca    = dto.Marca,
                    Modelo   = dto.Modelo,
                    Stock    = dto.Stock,
                    Diametro = dto.Diametro ?? 0,
                    Angulo   = dto.Angulo   ?? 0
                };

            default:
                throw new EquipoException("Debe especificarse un tipo de equipo valido (TELESCOPIO, MONTURA, CAMARA, OCULAR)");
        }
    }
}
