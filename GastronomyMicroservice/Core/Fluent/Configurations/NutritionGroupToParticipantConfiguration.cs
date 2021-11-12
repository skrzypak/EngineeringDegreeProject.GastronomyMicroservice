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
            modelBuilder.HasKey(a => new { a.Id, a.NutritionGroupId, a.ParticipantId, a.EspId });
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder
              .HasOne(n2p => n2p.NutritionGroup)
              .WithMany(n => n.NutritionsGroupsToParticipants)
              .HasForeignKey(n2p => new { n2p.NutritionGroupId, n2p.EspId })
              .HasPrincipalKey(n => new { n.Id, n.EspId });

            modelBuilder
              .HasOne(n2n => n2n.Participant)
              .WithMany(p => p.NutritionsGroupsToParticipants)
              .HasForeignKey(n2n => new { n2n.ParticipantId, n2n.EspId })
              .HasPrincipalKey(p => new { p.Id, p.EspId });

            modelBuilder.Property(a => a.StartDate).IsRequired();
            modelBuilder.Property(a => a.EndDate).IsRequired(false);
            modelBuilder.Property(a => a.NutritionGroupId).IsRequired();
            modelBuilder.Property(a => a.ParticipantId).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("NutritionGroupsToParticipants");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.StartDate).HasColumnName("StartDate");
            modelBuilder.Property(a => a.EndDate).HasColumnName("EndDate");
            modelBuilder.Property(a => a.NutritionGroupId).HasColumnName("NutritionGroupId");
            modelBuilder.Property(a => a.ParticipantId).HasColumnName("ParticipantId");
        }
    }
}
