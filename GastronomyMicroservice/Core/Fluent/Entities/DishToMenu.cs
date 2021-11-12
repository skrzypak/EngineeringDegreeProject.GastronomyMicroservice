using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Enums;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class DishToMenu : IEntity
    {
        public int Id { get; set; }
        public MealType Meal { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
