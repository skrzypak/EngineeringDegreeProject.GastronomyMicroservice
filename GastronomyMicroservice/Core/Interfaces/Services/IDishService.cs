using System.Collections.Generic;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IDishService
    {
        public object Get(int espId);
        public object GetById(int espId, int dishId);
        public object GetDishIngredients(int espId, int dishId);
        public object GetDishAllergens(int espId, int dishId);
        public int Create(int espId, int eudId, DishCoreDto<IngredientCoreDto> dto);
        public void Delete(int espId, int eudId, int dishId);
        public void CreateDishIngredients(int espId, int eudId, int dishId, ICollection<IngredientCoreDto> ingredients);
        public void DeleteDishIngredients(int espId, int eudId, int dishId, ICollection<int> ingredientsIds);
    }
}
