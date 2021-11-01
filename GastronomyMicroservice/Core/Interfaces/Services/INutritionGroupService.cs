using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionGroupService
    {
        public object Get();
        public object GetById(int nutiGrpId);
        public object GetNutritionPlans(int nutiGrpId, bool archive);
        public object GetParticipants(int nutiGrpId, bool archive);
        public int Create(NutritionGroupCoreDto<int> dto);
        public void AddParticipant(int nutiGrpId, ICollection<int> parcsIds);
        public void SetNutritionPlan(int nutiGrpId, int nutriPlsId, DateTime startDate, DateTime endDate);
        public void Delete(int nutiGrpId);
        public void RemoveNutritionPlan(int nutiGrpId,int nutiPlsId);
        public void RemoveParticipants(int nutiGrpId, ICollection<int> parcsId);
    }
}
