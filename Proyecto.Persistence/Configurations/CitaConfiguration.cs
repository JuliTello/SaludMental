using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Persistence.Configurations;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        // Nombre la tabla
        builder.ToTable("Citas");

        // Clave primaria
        builder.HasKey(x => x.CitaId);

        // Propiedades
        builder.Property(x => x.SessionDate).HasColumnType("date").IsRequired();
        builder.Property(x => x.Estado).IsRequired();

        //Relaciones
        builder.HasOne(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Especialista)
            .WithMany()
            .HasForeignKey(x => x.EspecialistaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
