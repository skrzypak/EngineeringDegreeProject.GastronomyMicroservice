using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionGroup
{
    public class NutritionGroupDto<TP, TNP> : NutritionGroupCoreDto<TP, TNP>
    {
        public int Id { get; set; }
    }
}
