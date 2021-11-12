using System;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionGroupService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int nutiGrpId);
        public object GetNutritionPlans(int enterpriseId, int nutiGrpId, bool archive);
        public object GetParticipants(int enterpriseId, int nutiGrpId, bool archive);
        public int Create(int enterpriseId, NutritionGroupCoreDto<int> dto);
        public void AddParticipant(int enterpriseId, int nutiGrpId, ICollection<int> parcsIds);
        public void SetNutritionPlan(int enterpriseId, int nutiGrpId, int nutriPlsId, DateTime startDate, DateTime endDate);
        public void Delete(int enterpriseId, int nutiGrpId);
        public void RemoveNutritionPlan(int enterpriseId, int nutiGrpId, int nutiGrpToNutiPlsId);
        public void RemoveParticipants(int enterpriseId, int nutiGrpId, ICollection<int> parcsId);
    }
}
