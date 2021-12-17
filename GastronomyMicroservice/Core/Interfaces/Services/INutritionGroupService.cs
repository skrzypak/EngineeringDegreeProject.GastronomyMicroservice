using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface INutritionGroupService
    {
        public object Get(int espId);
        public object GetById(int espId, int nutiGrpId);
        public object GetNutritionPlans(int espId, int nutiGrpId, bool archive);
        public object GetParticipants(int espId, int nutiGrpId, bool archive);
        public int Create(int espId, int eudId, NutritionGroupCoreDto<int, int> dto);
        public void AddParticipants(int espId, int eudId, int nutiGrpId, ICollection<ParticipantDatePair<int>> participants);
        public void SetNutritionPlan(int espId, int eudId, int nutiGrpId, int nutriPlsId, DateTime startDate, DateTime endDate);
        public void Delete(int espId, int eudId, int nutiGrpId);
        public void RemoveNutritionPlan(int espId, int eudId, int nutiGrpId, int nutiGrpToNutiPlsId);
        public void RemoveParticipants(int espId, int eudId, int nutiGrpId, ICollection<int> parcsId);
        public Task Update(int espId, int eudId, int nutiGrpId, NutritionGroupUpdateDto dto);
    }
}
