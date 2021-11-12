using System.Collections.Generic;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IDishService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int dishId);
        public object GetDishIngredients(int enterpriseId, int dishId);
        public object GetDishAllergens(int enterpriseId, int dishId);
        public int Create(int enterpriseId, DishCoreDto<IngredientCoreDto> dto);
        public void Delete(int enterpriseId, int dishId);
        public void CreateDishIngredients(int enterpriseId, int dishId, ICollection<IngredientCoreDto> ingredients);
        public void DeleteDishIngredients(int enterpriseId, int dishId, ICollection<int> ingredientsIds);
    }
}
