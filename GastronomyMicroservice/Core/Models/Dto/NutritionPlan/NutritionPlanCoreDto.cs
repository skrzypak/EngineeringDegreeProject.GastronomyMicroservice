using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Menu;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionPlan
{
    public class NutritionPlanCoreDto<TM>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TM> Menus { get; set; }
    }
}
