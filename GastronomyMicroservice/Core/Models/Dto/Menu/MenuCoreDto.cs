using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Fluent.Enums;

namespace GastronomyMicroservice.Core.Models.Dto.Menu
{
    public class MenuCoreDto<TD>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<DishMealPair<TD>> Dishes { get; set; }
    }

    public class DishMealPair<TD>
    {
        public TD Dish { get; set; }
        public MealType MealType { get; set; }
    }

}
