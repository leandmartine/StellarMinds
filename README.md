# StellarMinds

Proyecto final de la materia **Desarrollo Web Asistido con IA** (ORT Uruguay).

Sistema web para la gestión de un observatorio astronómico: usuarios, equipos, préstamos de instrumentación, observaciones y evaluación de adecuación equipo/objeto celeste mediante la API de **Google Gemini**.

## Demo en Somee

| Componente | URL |
|---|---|
| **Frontend (MVC)** | http://stellarmindsfrontend.somee.com/ |
| **API REST** | http://stellarmindsobg.somee.com/ |
| **Base de datos** | `StellarMindsBD.mssql.somee.com` (SQL Server en Somee) |

La aplicación desplegada en Somee utiliza la base de datos y la API alojadas en el mismo proveedor. No es necesario levantar servicios locales para probar la versión publicada.

## Contenidos del proyecto

Este repositorio integra los contenidos trabajados en la materia:

- **Componentes web**: arquitectura en capas con dominio, lógica de aplicación, acceso a datos, DTOs y capa de presentación MVC.
- **Manejo de interfaces**: contratos (`IRepositorio`, casos de uso, servicios) desacoplados de las implementaciones concretas.
- **Inversión de dependencias**: registro de servicios en `Program.cs` con inyección de dependencias de ASP.NET Core.
- **Principios SOLID**: entidades de dominio, value objects, casos de uso por responsabilidad y repositorios especializados.
- **CRUD completo**: altas, bajas, modificaciones y consultas sobre usuarios, equipos, préstamos, observaciones y objetos celestes.
- **Integración API Key Gemini**: evaluación automática de adecuación entre equipo astronómico y objeto celeste observado.
- **API REST**: autenticación JWT, endpoints documentados con Swagger y consumo desde el cliente MVC.

## Estructura del repositorio

```
StellarMinds/                  → Backend (.NET 10): dominio, lógica, EF Core, Web API
StellarMinds - WebCliente/     → Frontend MVC (.NET 10): consume la API REST
StellarMinds.sql               → Script de creación de la base de datos
```

### Capas del backend

| Proyecto | Descripción |
|---|---|
| `StellarMinds` | Entidades, value objects e interfaces de dominio/repositorio |
| `Excepciones` | Excepciones de dominio y aplicación |
| `LogicaAccesoDatos` | Entity Framework Core, DbContext y repositorios |
| `LogicaAplicacion` | Casos de uso e interfaces |
| `DTOs` | Objetos de transferencia y mappers |
| `StellarMindsWebAPI` | API REST con JWT, Swagger y servicio Gemini |

## Requisitos para ejecución local

- [.NET SDK 10](https://dotnet.microsoft.com/download) (ver `StellarMinds/global.json`)
- SQL Server o acceso a la base en Somee
- API Key de Google Gemini (para la funcionalidad de evaluación con IA)

## Ejecución local

### 1. Base de datos

Opción A — usar la base en Somee (recomendado para pruebas rápidas):

Configurar la cadena de conexión en `StellarMinds/StellarMindsWebAPI/appsettings.json`.

Opción B — base local:

Ejecutar el script `StellarMinds.sql` en SQL Server y apuntar `DefaultConnection` a esa instancia.

### 2. API REST

```bash
cd StellarMinds/StellarMindsWebAPI
dotnet restore
dotnet run
```

La API queda disponible en `http://localhost:5280`. Swagger: `http://localhost:5280/swagger`.

Configurar en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<cadena de conexión>"
  },
  "GeminiApiKey": "<tu API key de Gemini>",
  "SecretTokenKey": "<clave secreta para JWT>"
}
```

### 3. Cliente MVC

```bash
cd "StellarMinds - WebCliente/StellarMindsMVC"
dotnet restore
dotnet run
```

Verificar que `appsettings.json` apunte a la API:

```json
{
  "ApiBaseUrl": "http://localhost:5280/"
}
```

Para probar contra Somee, usar `"ApiBaseUrl": "http://stellarmindsobg.somee.com/"`.

## Usuarios de prueba

Usuarios precargados en la base de datos para testear los distintos roles:

| Rol | Usuario | Contraseña | Descripción |
|---|---|---|---|
| **Administrador** | `admin` | `Admin1@stellar` | Acceso total al sistema |
| **Coordinador** | `coord1` | `Coord1@2026!` | Gestión operativa del observatorio |
| **Socio** | `socio01` | `Socio01#2026` | Préstamos, observaciones y consultas propias |

> También existen usuarios `coord2`, `coord3` (coordinadores) y `socio02` a `socio10` (socios) con el mismo patrón de contraseña.

## Endpoints principales de la API

| Recurso | Ruta base |
|---|---|
| Usuarios | `/api/usuario` |
| Equipos | `/api/equipo` |
| Préstamos | `/api/prestamo` |
| Observaciones | `/api/observacion` |
| Objetos celestes | `/api/objetoceleste` |
| Auditoría | `/api/auditoria` |

La autenticación se realiza mediante JWT. Obtener el token con `POST /api/usuario/login`.

## Tecnologías

- ASP.NET Core 10 / C#
- Entity Framework Core + SQL Server
- ASP.NET MVC (cliente)
- JWT Bearer Authentication
- Swagger / OpenAPI
- Google Gemini API

## Autor

Leandro Martínez — ORT Uruguay, 3er Semestre.
