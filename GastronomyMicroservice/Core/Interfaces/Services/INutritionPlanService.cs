using System;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionPlanService
    {
        public object Get(int espId);
        public object GetById(int espId, int nutiPlsId);
        public int Create(int espId, int eudId, NutritionPlanCoreDto<int> dto);
        public int AddMenu(int espId, int eudId, int nutiPlsId, int menuId, uint order);
        public void RemoveMenu(int espId, int eudId, int nutiPlsId, int menuToPlsId);
        public void Delete(int espId, int eudId, int nutiPlsId);
        public void Update(int espId, int eudId, int nutiPlsId, NutritionPlanCoreDto<int> dto);
    }
}
