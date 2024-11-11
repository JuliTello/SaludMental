using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Persistence.Configurations;

public class EspecialistaConfiguration : IEntityTypeConfiguration<Especialista>
{
    public void Configure(EntityTypeBuilder<Especialista> builder)
    {
        // Nombre la tabla
        builder.ToTable("Especialistas");

        // Clave primaria
        builder.HasKey(x => x.EspecialistaId);

        // Propiedades
        builder.Property(x => x.UserName).HasMaxLength(30).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.Password).HasMaxLength(30).IsRequired().IsUnicode();
        builder.Property(x => x.Age).HasMaxLength(3).IsRequired();
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CodigoColegiatura).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Especialidad).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Info).HasMaxLength(200).IsRequired(false);
        builder.Property(x => x.Status).IsRequired();
    }
}