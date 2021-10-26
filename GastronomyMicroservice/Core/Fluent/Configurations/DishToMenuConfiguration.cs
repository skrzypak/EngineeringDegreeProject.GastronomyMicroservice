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
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.Meal).HasConversion<int>().IsRequired();
            modelBuilder.Property(a => a.DishId).IsRequired();
            modelBuilder.Property(a => a.MenuId).IsRequired();

            modelBuilder.ToTable("DishesToMenus");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.DishId).HasColumnName("DishId");
            modelBuilder.Property(a => a.MenuId).HasColumnName("MenuId");
        }
    }
}
