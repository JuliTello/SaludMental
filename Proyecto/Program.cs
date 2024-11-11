using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Proyecto.Persistence;
using Proyecto.Persistence.InitialData;
using Proyecto.Repositories.Implementations;
using Proyecto.Repositories.Interfaces;
using Proyecto.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.   
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("LocalDbConexion");
builder.Services.AddDbContext<ProyectoDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultUI()
                .AddEntityFrameworkStores<ProyectoDbContext>();


builder.Services.AddControllersWithViews();

// Servicios de Paginas de Razor
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUnitWork, UnitWork>();

// Servicio de EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Servicio de Datos Iniciales
builder.Services.AddScoped<IDbInitialize, DbInitialize>();

var app = builder.Build();

// Datos Iniciales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var inicializador = services.GetRequiredService<IDbInitialize>();
        inicializador.Initialize();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Un error ocurrió el ejecutar la migración.");
    }
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
