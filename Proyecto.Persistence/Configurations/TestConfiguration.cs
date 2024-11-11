using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Persistence.Configurations;

public class TestConfiguration: IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        // Nombre la tabla
        builder.ToTable("Tests");

        // Clave primaria
        builder.HasKey(x => x.TestId);

        // Propiedades
        builder.Property(x => x.NombreTest).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.Pregunta1).IsRequired();
        builder.Property(x => x.Pregunta2).IsRequired();
        builder.Property(x => x.Pregunta3).IsRequired();
        builder.Property(x => x.Pregunta4).IsRequired();
        builder.Property(x => x.Pregunta5).IsRequired();
    }
}
