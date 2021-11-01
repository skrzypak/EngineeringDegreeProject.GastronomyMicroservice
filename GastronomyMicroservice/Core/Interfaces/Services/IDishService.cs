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
        public object GetDishes();
        public object GetDishById(int dishId);
        public object GetDishIngredients(int dishId);
        public object GetDishAllergens(int dishId);
        public int CreateDish(DishCoreDto<IngredientCoreDto> dto);
        public void DeleteDish(int dishId);
        public void AddDishIngredients(int dishId, ICollection<IngredientCoreDto> ingredients);
        public void DeleteDishIngredients(int dishId, ICollection<int> ingredientsIds);
    }
}
