using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class NutritionGroupToParticipantConfiguration : IEntityTypeConfiguration<NutritionGroupToParticipant>
    {
        public void Configure(EntityTypeBuilder<NutritionGroupToParticipant> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.StartDate).IsRequired();
            modelBuilder.Property(a => a.EndDate).IsRequired(false);
            modelBuilder.Property(a => a.NutritionGroupId).IsRequired();
            modelBuilder.Property(a => a.ParticipantId).IsRequired();

            modelBuilder.ToTable("NutritionGroupsToParticipants");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.StartDate).HasColumnName("StartDate");
            modelBuilder.Property(a => a.EndDate).HasColumnName("EndDate");
            modelBuilder.Property(a => a.NutritionGroupId).HasColumnName("NutritionGroupId");
            modelBuilder.Property(a => a.ParticipantId).HasColumnName("ParticipantId");
        }
    }
}
