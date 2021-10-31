using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class NutritionPlan : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<NutritionGroupToNutritionPlan> NutritionsGroupsToNutritionsPlans { get; set; }
        public virtual ICollection<MenuToNutritonPlan> MenusToNutritonsPlans { get; set; }
    }
}
