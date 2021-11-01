﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IMenuService
    {
        public object Get();
        public object GetById(int menuId);
        public object GetMenuDishes(int menuId);
        public object GetMenuAllergens(int menuId);
        public int Create(MenuCoreDto<int> dto);
        public ICollection<int> SetDishesToMenu(int menuId, ICollection<int> dishesIds); // TODO
        public void Delete(int menuId);
        public void RemoveDishesFromMenu(int menuId, ICollection<int> dishesIds);
    }
}
