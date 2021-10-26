using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.FirstName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(a => a.LastName).HasMaxLength(300).IsRequired();
            modelBuilder.Ignore(a => a.FullName);
            modelBuilder.Property(a => a.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.ToTable("Participants");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.FirstName).HasColumnName("FirstName");
            modelBuilder.Property(a => a.LastName).HasColumnName("LastName");
            modelBuilder.Property(a => a.Description).HasColumnName("Description");
        }
    }
}
