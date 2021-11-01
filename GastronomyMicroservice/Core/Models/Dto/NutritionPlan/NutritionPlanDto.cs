using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionPlan
{
    public class NutritionPlanDto<TM, TNG> : NutritionPlanCoreDto<TM>
    {
        public int Id { get; set; }
        public TNG NutritionGroup { get; set; }
    }
}
