using System.Collections.Generic;
using GastronomyMicroservice.Core.Models.Dto.Menu;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IMenuService
    {
        public object Get(int espId);
        public object GetById(int espId, int menuId);
        public int Create(int espId, int eudId, MenuCoreDto<int> dto);
        public ICollection<int> SetDishesToMenu(int espId, int eudId, int menuId, ICollection<DishMealPair<int>> dishMealPairs);
        public void Delete(int espId, int eudId, int menuId);
        public void RemoveDishesFromMenu(int espId, int eudId, int menuId, ICollection<int> menuDishesIds);
        public void Update(int espId, int eudId, int menuId, MenuCoreDto<int> dto);
    }
}
