using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionPlan
{
    public class NutritionPlanDto<TM> : NutritionPlanCoreDto<TM>
    {
        public int Id { get; set; }
        public int NutritionGroupId { get; set; }
    }
}
