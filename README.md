# StellarMinds

Proyecto final de la materia **Desarrollo Web Asistido con IA** (ORT Uruguay).

Sistema web para la gestión de un observatorio astronómico: usuarios, equipos, préstamos de instrumentación, observaciones y evaluación de adecuación equipo/objeto celeste mediante la API de **Google Gemini**.

## Demo en Somee

| Componente | URL |
|---|---|
| **Frontend (MVC)** | http://frontstellarminds.somee.com/ |
| **API REST** | http://stellarmindsobg.somee.com/ |
| **Base de datos** | `StellarMindsBD.mssql.somee.com` (SQL Server en Somee) |

La aplicación desplegada en Somee utiliza la base de datos y la API alojadas en el mismo proveedor. No es necesario levantar servicios locales para probar la versión publicada.

### Si las URLs de Somee no responden

El plan free de Somee puede dejar de servir el sitio por **inactividad**, mantenimiento del proveedor u otros límites del servicio.  
Si **no podés acceder** al frontend, a la API o a la base remota, **ejecutá el setup local** de este README (sección [Ejecución 100% local](#ejecución-100-local)). Ahí tenés todo lo necesario para correr el proyecto de forma autónoma en tu máquina.

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
StellarMinds.sql               → Script de datos / seed de la base de datos
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

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (ver `StellarMinds/global.json`; se recomienda roll-forward a un patch reciente)
- **SQL Server** local (SQL Server Express, Developer o **LocalDB**) — para modo 100% local
- API Key de [Google Gemini](https://aistudio.google.com/apikey) (solo necesaria para la evaluación con IA en observaciones; el resto del sistema funciona sin ella)
- (Opcional) Cliente SQL: Azure Data Studio, SSMS o `sqlcmd`

---

## Ejecución 100% local

Esta es la forma de correr **todo el stack en tu máquina** (SQL + API + frontend), sin depender de Somee. Usala siempre que las demos remotas fallen o para desarrollo.

### Resumen de puertos

| Servicio | URL local |
|---|---|
| API REST | http://localhost:5280 |
| Swagger | http://localhost:5280/swagger |
| Frontend MVC | http://localhost:64796 |

Orden: **1) base de datos → 2) API → 3) frontend**.

### Paso 1 — Crear y cargar la base de datos local

1. Asegurate de tener SQL Server o LocalDB en ejecución.
2. Creá la base (si no existe):

```sql
CREATE DATABASE StellarMinds;
```

3. Generá el esquema con EF Core (desde la carpeta del backend):

```bash
cd StellarMinds/StellarMindsWebAPI
dotnet restore
dotnet tool install --global dotnet-ef   # solo la primera vez
dotnet ef database update --project ../LogicaAccesoDatos --startup-project .
```

> Si `dotnet ef` no encuentra el comando, cerrá y reabrí la terminal o usá  
> `dotnet tool run dotnet-ef` según tu instalación.

4. Cargá los datos de prueba ejecutando el script **`StellarMinds.sql`** (en la raíz del repo) contra la base `StellarMinds`  
   (en Azure Data Studio / SSMS: abrir el archivo y ejecutar; el script hace `USE StellarMinds` e inserta usuarios, equipos, etc.).

**Cadenas de conexión de ejemplo** (elegí la que coincida con tu instalación):

```text
# LocalDB (común en Windows / VS)
Server=(localdb)\mssqllocaldb;Database=StellarMinds;Trusted_Connection=True;TrustServerCertificate=True

# SQL Server Express local
Server=localhost\SQLEXPRESS;Database=StellarMinds;Trusted_Connection=True;TrustServerCertificate=True

# SQL Server con usuario/clave
Server=localhost,1433;Database=StellarMinds;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True
```

### Paso 2 — Configurar la API (modificaciones en appsettings)

Archivo: `StellarMinds/StellarMindsWebAPI/appsettings.json`

Reemplazá los placeholders por valores locales reales:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StellarMinds;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "GeminiApiKey": "TU_API_KEY_DE_GEMINI",
  "SecretTokenKey": "ClaveSecretaLocalDev_Minimo32Caracteres!!"
}
```

| Clave | Qué poner en local |
|---|---|
| `DefaultConnection` | Cadena a **tu** SQL Server local (ver ejemplos arriba) |
| `GeminiApiKey` | Key de Gemini; si no tenés, el resto del sistema sigue andando (falla solo la evaluación IA) |
| `SecretTokenKey` | Cualquier string **largo y secreto** para firmar JWT (no uses el placeholder de producción) |

> Tip: podés dejar el `appsettings.json` de demo/Somee y crear `appsettings.Development.json` con estos valores; en `Development` ASP.NET Core los combina y pisan los de base.  
> Con `dotnet run` el perfil por defecto ya usa `ASPNETCORE_ENVIRONMENT=Development`.

### Paso 3 — Levantar la API

```bash
cd StellarMinds/StellarMindsWebAPI
dotnet restore
dotnet run --launch-profile http
```

Verificá:

- http://localhost:5280/swagger  
- Si Swagger abre, la API está lista.

Dejá esta terminal abierta.

### Paso 4 — Configurar el frontend (modificación clave)

Archivo: `StellarMinds - WebCliente/StellarMindsMVC/appsettings.json`

Para modo **100% local**, la URL de la API debe apuntar a tu máquina (no a Somee):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiBaseUrl": "http://localhost:5280/"
}
```

| Valor de `ApiBaseUrl` | Cuándo usarlo |
|---|---|
| `http://localhost:5280/` | **Local completo** (API en tu PC) |
| `http://stellarmindsobg.somee.com/` | Frontend local consumiendo la API de Somee (si la API remota responde) |

La barra final `/` es importante.

### Paso 5 — Levantar el frontend MVC

En **otra terminal**:

```bash
cd "StellarMinds - WebCliente/StellarMindsMVC"
dotnet restore
dotnet run
```

Abrí el navegador en:

- **Landing / inicio:** http://localhost:64796/  
- **Login:** http://localhost:64796/Usuario/Login  

Si el perfil lanza HTTPS y da problemas de certificado, podés forzar solo HTTP:

```bash
ASPNETCORE_URLS="http://localhost:64796" ASPNETCORE_ENVIRONMENT=Development dotnet run --no-launch-profile
```

### Checklist rápido (local)

- [ ] SQL Server / LocalDB corriendo  
- [ ] Base `StellarMinds` creada + migraciones + `StellarMinds.sql` ejecutado  
- [ ] API `appsettings` con `DefaultConnection` y `SecretTokenKey` locales  
- [ ] API en http://localhost:5280 (Swagger OK)  
- [ ] MVC `ApiBaseUrl` = `http://localhost:5280/`  
- [ ] Frontend en http://localhost:64796  
- [ ] Login con un usuario de prueba (tabla abajo)

### Problemas frecuentes en local

| Síntoma | Qué revisar |
|---|---|
| Error de conexión a SQL al arrancar la API | Cadena `DefaultConnection`, instancia LocalDB/Express, que la BD exista |
| Login falla / 500 en el front | Que la API esté arriba y `ApiBaseUrl` apunte a `http://localhost:5280/` |
| Gemini / adecuación falla | `GeminiApiKey` vacía o inválida (el resto del CRUD puede seguir OK) |
| Puerto en uso | Cambiá el puerto en `Properties/launchSettings.json` o con `ASPNETCORE_URLS` |
| CORS | La API ya permite cualquier origen en desarrollo (`AllowAnyOrigin`) |

---

## Ejecución híbrida (opcional)

Si **solo** Somee frontend falla pero la **API remota** sigue viva:

1. Configurá el MVC local con `"ApiBaseUrl": "http://stellarmindsobg.somee.com/"`.
2. Corré solo el frontend (`dotnet run` en `StellarMindsMVC`).

Si la API remota también está caída, usá el modo **100% local** de arriba.

---

## Usuarios de prueba

Usuarios precargados en la base de datos (script `StellarMinds.sql`) para testear los distintos roles:

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
