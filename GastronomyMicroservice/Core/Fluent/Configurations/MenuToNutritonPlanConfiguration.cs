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
            modelBuilder.HasKey(a => new { a.Id, a.MenuId, a.NutritionPlanId, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedNever().IsRequired();

            modelBuilder
               .HasOne(m2n => m2n.Menu)
               .WithMany(m => m.MenusToNutritonsPlans)
               .HasForeignKey(m2n => new { m2n.MenuId, m2n.EspId })
               .HasPrincipalKey(m => new { m.Id, m.EspId });

            modelBuilder
              .HasOne(m2n => m2n.NutritionPlan)
              .WithMany(d => d.MenusToNutritonsPlans)
              .HasForeignKey(m2n => new { m2n.NutritionPlanId, m2n.EspId })
              .HasPrincipalKey(d => new { d.Id, d.EspId });

            modelBuilder.Property(a => a.TargetDate).IsRequired();
            modelBuilder.Property(a => a.MenuId).IsRequired();
            modelBuilder.Property(a => a.NutritionPlanId).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("MenusToNutritonPlans");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.TargetDate).HasColumnName("TargetDate");
            modelBuilder.Property(a => a.MenuId).HasColumnName("MenuId");
            modelBuilder.Property(a => a.NutritionPlanId).HasColumnName("NutritionPlanId");
        }
    }
}
