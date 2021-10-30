using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionGroup
{
    public class NutritionGroupCoreDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<int> ParticipantsIds { get; set; }
    }
}
