using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class DishToMenuConfiguration : IEntityTypeConfiguration<DishToMenu>
    {
        public void Configure(EntityTypeBuilder<DishToMenu> modelBuilder)
        {
            modelBuilder.HasKey(a => new { a.Id, a.DishId, a.MenuId, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder
               .HasOne(d2m => d2m.Dish)
               .WithMany(d => d.DishsToMenus)
               .HasForeignKey(dtm => new { dtm.DishId, dtm.EspId })
               .HasPrincipalKey(d => new { d.Id, d.EspId });

            modelBuilder
               .HasOne(d2m => d2m.Menu)
               .WithMany(m => m.DishsToMenus)
               .HasForeignKey(dtm => new { dtm.MenuId, dtm.EspId })
               .HasPrincipalKey(m => new { m.Id, m.EspId });

            modelBuilder.Property(a => a.Meal).HasConversion<int>().IsRequired();
            modelBuilder.Property(a => a.DishId).IsRequired();
            modelBuilder.Property(a => a.MenuId).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("DishesToMenus");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.DishId).HasColumnName("DishId");
            modelBuilder.Property(a => a.MenuId).HasColumnName("MenuId");
        }
    }
}
