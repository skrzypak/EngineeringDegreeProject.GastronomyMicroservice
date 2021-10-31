using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class MenuToNutritonPlan : IEntity
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public int NutritionPlanId { get; set; }
        public virtual NutritionPlan NutritionPlan { get; set; }
    }
}
