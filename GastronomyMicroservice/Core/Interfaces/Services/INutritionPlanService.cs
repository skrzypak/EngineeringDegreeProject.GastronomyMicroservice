using System;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionPlanService
    {
        public object Get();
        public object GetById(int nutiPlsId);
        public int Create(NutritionPlanCoreDto<int> dto);
        public int AddMenu(int nutiPlsId, int menuId, DateTime targetDate);
        public void RemoveMenu(int nutiPlsId, int menuToPlsId);
        public void Delete(int nutiPlsId);
    }
}
