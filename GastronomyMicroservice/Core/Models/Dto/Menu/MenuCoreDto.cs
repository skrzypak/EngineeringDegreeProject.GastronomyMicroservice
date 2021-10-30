using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;

namespace GastronomyMicroservice.Core.Models.Dto.Menu
{
    public class MenuCoreDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<int> DishesId { get; set; }
        public virtual ICollection<DishCoreDto<IngredientCoreDto>> Dishes { get; set; }
    }
}
