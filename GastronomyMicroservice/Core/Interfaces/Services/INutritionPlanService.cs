using System;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionPlanService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int nutiPlsId);
        public int Create(int enterpriseId, NutritionPlanCoreDto<int> dto);
        public int AddMenu(int enterpriseId, int nutiPlsId, int menuId, DateTime targetDate);
        public void RemoveMenu(int enterpriseId, int nutiPlsId, int menuToPlsId);
        public void Delete(int enterpriseId, int nutiPlsId);
    }
}
