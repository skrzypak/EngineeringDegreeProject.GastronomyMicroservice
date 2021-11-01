using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IMenuService
    {
        public object GetMenus();
        public object GetById(int menuId);
        public object GetMenuDishes(int menuId);
        public object GetMenuAllergens(int menuId);
        public int CreateMenu(MenuCoreDto<int> dto);
        public ICollection<int> SetDishesToMenu(int menuId, ICollection<int> dishesIds);
        public void DeleteMenu(int menuId);
        public void RemoveDishesFromMenu(int menuId, ICollection<int> dishesIds);
    }
}
