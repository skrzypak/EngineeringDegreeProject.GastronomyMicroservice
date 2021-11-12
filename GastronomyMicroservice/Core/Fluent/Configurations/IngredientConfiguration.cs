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
            modelBuilder.HasKey(a => new { a.Id, a.DishId, a.ProductId, a.EspId });
            modelBuilder.Property(a => a.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder
               .HasOne(i => i.Product)
               .WithMany(p => p.Ingredients)
               .HasForeignKey(i => new { i.ProductId, i.EspId })
               .HasPrincipalKey(p => new { p.Id, p.EspId });

            modelBuilder
               .HasOne(i => i.Dish)
               .WithMany(d => d.Ingredients)
               .HasForeignKey(i => new { i.DishId, i.EspId })
               .HasPrincipalKey(d => new { d.Id, d.EspId });

            modelBuilder.Property(a => a.ValueOfUse).IsRequired();
            modelBuilder.Property(a => a.ProductId).IsRequired();
            modelBuilder.Property(a => a.DishId).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("Ingredients");
            modelBuilder.Property(a => a.Id).HasColumnName("Id");
            modelBuilder.Property(a => a.ValueOfUse).HasColumnName("ValueOfUse");
            modelBuilder.Property(a => a.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(a => a.DishId).HasColumnName("DishId");
        }
    }
}
