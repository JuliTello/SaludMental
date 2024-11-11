using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Persistence.Configurations;

public class TestDetailConfiguration: IEntityTypeConfiguration<TestDetail>
{
    public void Configure(EntityTypeBuilder<TestDetail> builder)
    {
        // Nombre la tabla
        builder.ToTable("TestDetails");

        // Clave primaria
        builder.HasKey(x => x.TestDetailId);

        // Propiedades
        builder.Property(x => x.FechaRealizacion).HasColumnType("date").IsRequired();
        builder.Property(x => x.Resultado).HasMaxLength(100).IsRequired();

        //Relaciones
        builder.HasOne(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.ApplicationUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Test)
            .WithMany()
            .HasForeignKey(x => x.TestId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
