using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class NutritionPlan : IEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int NutritionGroupId { get; set; }
        public virtual NutritionGroup NutritionGroup { get; set; }
        public virtual ICollection<MenuToNutritonPlan> MenusToNutritonsPlans { get; set; }
    }
}
