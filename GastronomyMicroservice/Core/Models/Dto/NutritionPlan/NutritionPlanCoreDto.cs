using System;
using System.Collections.Generic;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionPlan
{
    public class NutritionPlanCoreDto<TM>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<MenuDatePair<TM>> Menus { get; set; }
    }

    public class MenuDatePair<TM>
    {
        public TM Menu { get; set; }
        public uint Order { get; set; }
    }
}
