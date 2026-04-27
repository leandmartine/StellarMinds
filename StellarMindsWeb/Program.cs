using LogicaAccesoDatos.RepositorioMemoria;
using LogicaAplicacion.CasosDeUso.CUEquipos;
using LogicaAplicacion.CasosDeUso.CUUsuario;
using LogicaAplicacion.InterfacesCasoDeUso.Equipos;
using LogicaAplicacion.InterfacesCasoDeUso.Usuarios;
using StellarMinds.InterfacesRepositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Repositorios
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioEquipo, RepositorioEquipo>();

// Casos de uso - Usuarios
builder.Services.AddScoped<IAltaUsuario, AltaUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarios, ObtenerUsuarioCU>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuarioCU>();
builder.Services.AddScoped<ILogOutUsuario, LogOutUsuarioCU>();
builder.Services.AddScoped<IObtenerUsuarioPorId, ObtenerUsuarioPorIdCU>();

// Casos de uso - Equipos (RF03)
builder.Services.AddScoped<IAltaEquipo, AltaEquipo>();
builder.Services.AddScoped<IModificarEquipo, ModificarEquipo>();
builder.Services.AddScoped<IBajaEquipo, BajaEquipo>();
builder.Services.AddScoped<IObtenerEquipos, ObtenerEquipo>();
builder.Services.AddScoped<IObtenerEquipoPorId, ObtenerEquipoPorId>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Usuario}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
