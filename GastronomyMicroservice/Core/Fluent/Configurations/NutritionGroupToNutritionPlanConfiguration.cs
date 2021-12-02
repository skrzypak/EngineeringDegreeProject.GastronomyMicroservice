using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class NutritionGroupToNutritionPlanConfiguration : IEntityTypeConfiguration<NutritionGroupToNutritionPlan>
    {
        public void Configure(EntityTypeBuilder<NutritionGroupToNutritionPlan> modelBuilder)
        {
            modelBuilder.HasKey(a => new { a.Id, a.NutritionGroupId, a.NutritionPlanId, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(a => a.NutritionGroupId).IsRequired();
            modelBuilder.Property(a => a.NutritionPlanId).IsRequired();

            modelBuilder
              .HasOne(n2n => n2n.NutritionGroup)
              .WithMany(n => n.NutritionsGroupsToNutritionsPlans)
              .HasForeignKey(n2n => new { n2n.NutritionGroupId, n2n.EspId })
              .HasPrincipalKey(n => new { n.Id, n.EspId });

            modelBuilder
              .HasOne(n2n => n2n.NutritionPlan)
              .WithMany(n => n.NutritionsGroupsToNutritionsPlans)
              .HasForeignKey(n2n => new { n2n.NutritionPlanId, n2n.EspId })
              .HasPrincipalKey(n => new { n.Id, n.EspId });

            modelBuilder.Property(a => a.StartDate).IsRequired();
            modelBuilder.Property(a => a.EndDate).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("NutritionGroupsToNutritionPlans");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.NutritionGroupId).HasColumnName("NutritionGroupId");
            modelBuilder.Property(a => a.NutritionPlanId).HasColumnName("NutritionPlanId");
            modelBuilder.Property(a => a.StartDate).HasColumnName("StartDate");
            modelBuilder.Property(a => a.EndDate).HasColumnName("EndDate");
        }
    }
}
