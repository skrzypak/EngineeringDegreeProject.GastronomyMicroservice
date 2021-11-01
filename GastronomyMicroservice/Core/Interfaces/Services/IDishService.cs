using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IDishService
    {
        public object Get();
        public object GetById(int dishId);
        public object GetDishIngredients(int dishId);
        public object GetDishAllergens(int dishId);
        public int Create(DishCoreDto<IngredientCoreDto> dto);
        public void Delete(int dishId);
        public void CreateDishIngredients(int dishId, ICollection<IngredientCoreDto> ingredients);
        public void DeleteDishIngredients(int dishId, ICollection<int> ingredientsIds);
    }
}
