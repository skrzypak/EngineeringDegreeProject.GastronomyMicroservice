using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class NutritionGroupConfiguration : IEntityTypeConfiguration<NutritionGroup>
    {
        public void Configure(EntityTypeBuilder<NutritionGroup> modelBuilder)
        {
            modelBuilder.HasKey(a => new { a.Id, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedNever().IsRequired();

            modelBuilder.Property(a => a.Name).HasMaxLength(300).IsRequired();
            modelBuilder.Property(a => a.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("NutritionGroups");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.Name).HasColumnName("Name");
            modelBuilder.Property(a => a.Description).HasColumnName("Description");
        }
    }
}
