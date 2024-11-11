using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;
using System.Reflection;

namespace Proyecto.Persistence;

public class ProyectoDbContext : IdentityDbContext
{
    public ProyectoDbContext(DbContextOptions<ProyectoDbContext> options) : base(options)
    { }
    public DbSet<Especialista> Especialistas { get; set; }
    public DbSet<Cita> Citas { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Mensajeria> Mensajerias { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestDetail> TestsDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // La configuración del modelo esta en un archivo externo
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}