using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proyecto.Models;


namespace Proyecto.Persistence.InitialData;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ProyectoDbContext(serviceProvider.GetRequiredService<DbContextOptions<ProyectoDbContext>>()))
        {
          
            //if (context.Especialistas.Any())
            //    return;

            //context.Especialistas.Add(new Especialista { UserName = "Carlos", PhoneNumber = "999 222 123", Email = "carlos@correo.com", Password = "carlos123*", Age = 34, 
            //    Gender = "Masculino", FirstName = "Carlos", LastName = "Quispe", CodigoColegiatura = "A124", Especialidad = "Psicólogo" });

            //context.SaveChanges();
        }
    }
}
