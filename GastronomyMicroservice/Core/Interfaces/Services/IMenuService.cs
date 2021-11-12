using System.Collections.Generic;
using GastronomyMicroservice.Core.Models.Dto.Menu;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IMenuService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int menuId);
        public int Create(int enterpriseId, MenuCoreDto<int> dto);
        public ICollection<int> SetDishesToMenu(int enterpriseId, int menuId, ICollection<DishMealPair<int>> dishMealPairs);
        public void Delete(int enterpriseId, int menuId);
        public void RemoveDishesFromMenu(int enterpriseId, int menuId, ICollection<int> dishesIds);
    }
}
