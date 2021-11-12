using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Configurations;
using GastronomyMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;

namespace GastronomyMicroservice.Core.Fluent
{
    public class MicroserviceContext : DbContext
    {
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<AllergenToProduct> AllergensToProducts { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishToMenu> DishToMenus { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuToNutritonPlan> MenusToNutritonPlans { get; set; }
        public DbSet<NutritionGroup> NutritionGroups { get; set; }
        public DbSet<NutritionGroupToParticipant> NutritionGroupsToParticipants { get; set; }
        public DbSet<NutritionGroupToNutritionPlan> NutritionsGroupsToNutritionsPlans { get; set; }
        public DbSet<NutritionPlan> NutritionPlans { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Product> Products { get; set; }

        public MicroserviceContext(DbContextOptions options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.AddProperty("CreatedDate", typeof(DateTime));
                entity.AddProperty("LastUpdatedDate", typeof(DateTime?));
            }

            modelBuilder.ApplyConfiguration(new AllergenConfiguration());
            modelBuilder.ApplyConfiguration(new AllergenToProductConfiguration());
            modelBuilder.ApplyConfiguration(new IngredientConfiguration());
            modelBuilder.ApplyConfiguration(new DishConfiguration());
            modelBuilder.ApplyConfiguration(new DishToMenuConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
            modelBuilder.ApplyConfiguration(new MenuToNutritonPlanConfiguration());
            modelBuilder.ApplyConfiguration(new NutritionGroupConfiguration());
            modelBuilder.ApplyConfiguration(new NutritionGroupToParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new NutritionGroupToNutritionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new NutritionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
        #endregion

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("LastUpdatedDate").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}
