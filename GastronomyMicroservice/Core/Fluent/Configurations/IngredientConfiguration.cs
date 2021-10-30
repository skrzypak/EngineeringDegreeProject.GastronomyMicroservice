using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GastronomyMicroservice.Core.Fluent.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> modelBuilder)
        {
            modelBuilder.HasKey(a => a.Id);
            modelBuilder.Property(a => a.Id).IsRequired();

            modelBuilder.Property(a => a.ValueOfUse).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();
            modelBuilder.Property(a => a.DishId).IsRequired();

            modelBuilder.ToTable("Ingredients");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.ValueOfUse).HasColumnName("ValueOfUse");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(a => a.DishId).HasColumnName("DishId");
        }
    }
}
