using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class MenuToNutritonPlanConfiguration : IEntityTypeConfiguration<MenuToNutritonPlan>
    {
        public void Configure(EntityTypeBuilder<MenuToNutritonPlan> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.Date).IsRequired();
            modelBuilder.Property(a => a.MenuId).IsRequired();
            modelBuilder.Property(a => a.NutritionPlanId).IsRequired();

            modelBuilder.ToTable("MenusToNutritonPlans");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.Date).HasColumnName("Date");
            modelBuilder.Property(a => a.MenuId).HasColumnName("MenuId");
            modelBuilder.Property(a => a.NutritionPlanId).HasColumnName("NutritionPlanId");
        }
    }
}
