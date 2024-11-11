using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Persistence.Configurations;

public class MensajeriaConfiguration : IEntityTypeConfiguration<Mensajeria>
{
    public void Configure(EntityTypeBuilder<Mensajeria> builder)
    {
        // Nombre la tabla
        builder.ToTable("Mensajerias");

        // Clave primaria
        builder.HasKey(x => x.MensajeriaId);

        // Propiedades
        builder.Property(x => x.Mensaje).HasMaxLength(500).IsRequired();
        builder.Property(x => x.FechaEnvio).HasColumnType("date").IsRequired();

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
