using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Ingredient
{
    public class IngredientDto : IngredientCoreDto
    {
        public int Id { get; set; }
        public int DishId { get; set; }
    }
}
