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
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.NutritionGroupId).IsRequired();
            modelBuilder.Property(a => a.NutritionPlanId).IsRequired();

            modelBuilder.Property(a => a.StartDate).IsRequired();
            modelBuilder.Property(a => a.EndDate).IsRequired();

            modelBuilder.ToTable("NutritionGroupsToNutritionPlans");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.NutritionGroupId).HasColumnName("NutritionGroupId");
            modelBuilder.Property(a => a.NutritionPlanId).HasColumnName("NutritionPlanId");
            modelBuilder.Property(a => a.StartDate).HasColumnName("StartDate");
            modelBuilder.Property(a => a.EndDate).HasColumnName("EndDate");
        }
    }
}
