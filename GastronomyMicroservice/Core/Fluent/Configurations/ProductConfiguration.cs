﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.Code).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(a => a.Name).HasMaxLength(300).IsRequired();
            modelBuilder.Property(a => a.Unit).HasConversion<string>().HasMaxLength(10).IsRequired();
            modelBuilder.Property(a => a.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.ToTable("Products");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.Name).HasColumnName("Name");
            modelBuilder.Property(a => a.Unit).HasColumnName("Unit");
            modelBuilder.Property(a => a.Description).HasColumnName("Description");
        }
    }
}
