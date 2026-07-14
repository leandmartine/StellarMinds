using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LogicaAccesoDatos.EntityFramework;
using LogicaAccesoDatos.EntityFramework.Repositorios;
using LogicaAplicacion.CasosDeUso.CUEquipos;
using LogicaAplicacion.CasosDeUso.CUObjetosCelestes;
using LogicaAplicacion.CasosDeUso.CUObservaciones;
using LogicaAplicacion.CasosDeUso.CUPrestamos;
using LogicaAplicacion.CasosDeUso.CUUsuario;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using LogicaAplicacion.InterfacesCasoDeUso.ObjetosCelestes;
using LogicaAplicacion.InterfacesCasoDeUso.Observaciones;
using LogicaAplicacion.InterfacesCasoDeUso.Prestamos;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using LogicaAplicacion.Servicios;
using StellarMinds.InterfacesRepositorios;
using StellarMindsWebAPI;
using StellarMindsWebAPI.Servicios;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddCors(opts => opts.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// DbContext con SQL Server (LocalDB)
builder.Services.AddDbContext<StellarMindsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios EF
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuarioEF>();
builder.Services.AddScoped<IRepositorioEquipo, RepositorioEquipoEF>();
builder.Services.AddScoped<IRepositorioPrestamo, RepositorioPrestamoEF>();
builder.Services.AddScoped<IRepositorioObservacion, RepositorioObservacionEF>();
builder.Services.AddScoped<IRepositorioObjetoCeleste, RepositorioObjetoCelesteEF>();
builder.Services.AddScoped<IRepositorioAuditoriaPrestamo, RepositorioAuditoriaPrestamoEF>();

// Casos de uso - Usuarios
builder.Services.AddScoped<IAltaUsuario, AltaUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarios, ObtenerUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuarioCU>();
builder.Services.AddScoped<ILogOutUsuario, LogOutUsuarioCU>();

// Casos de uso - Equipos
builder.Services.AddScoped<IAltaEquipo, AltaEquipo>();
builder.Services.AddScoped<IModificarEquipo, ModificarEquipo>();
builder.Services.AddScoped<IBajaEquipo, BajaEquipo>();
builder.Services.AddScoped<IObtenerEquipos, ObtenerEquipo>();
builder.Services.AddScoped<IObtenerEquipoPorId, ObtenerEquipoPorId>();

// Casos de uso - Prestamos
builder.Services.AddScoped<IAltaPrestamo, AltaPrestamo>();
builder.Services.AddScoped<IBajaPrestamo, BajaPrestamo>();
builder.Services.AddScoped<IObtenerPrestamos, ObtenerPrestamo>();
builder.Services.AddScoped<IObtenerPrestamosPorSocio, ObtenerPrestamosPorSocio>();
builder.Services.AddScoped<IObtenerPrestamosPorPeriodo, ObtenerPrestamosPorPeriodo>();
builder.Services.AddScoped<IObtenerSociosPorTelescopio, ObtenerSociosPorTelescopio>();
builder.Services.AddScoped<IObtenerAuditoriaPrestamos, ObtenerAuditoriaPrestamos>();

// Casos de uso - Observaciones
builder.Services.AddScoped<IAltaObservacion, AltaObservacion>();
builder.Services.AddScoped<IObtenerObservaciones, ObtenerObservacion>();

// Casos de uso - Objetos Celestes
builder.Services.AddScoped<IObtenerObjetosCelestes, ObtenerObjetosCelestes>();
builder.Services.AddScoped<IObtenerRankingObjetosCelestes, ObtenerRankingObjetosCelestes>();

// Servicios externos
builder.Services.AddScoped<IServicioGemini, ServicioGemini>();

// Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.ASCII.GetBytes(builder.Configuration.GetSection("SecretTokenKey").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar migraciones pendientes al iniciar la aplicación
using (IServiceScope scope = app.Services.CreateScope())
{
    StellarMindsContext context = scope.ServiceProvider.GetRequiredService<StellarMindsContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
