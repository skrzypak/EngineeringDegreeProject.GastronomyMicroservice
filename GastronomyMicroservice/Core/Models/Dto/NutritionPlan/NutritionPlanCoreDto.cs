using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Menu;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionPlan
{
    public class NutritionPlanCoreDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ICollection<int> MenusIds { get; set; }
        public ICollection<MenuCoreDto> Menus { get; set; }
    }
}
