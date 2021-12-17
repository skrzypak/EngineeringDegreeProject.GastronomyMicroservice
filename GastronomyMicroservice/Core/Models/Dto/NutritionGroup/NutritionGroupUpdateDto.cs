using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionGroup
{
    public class NutritionGroupUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ParticipantDatePair<int>> ParticipantsToAdd { get; set; }
        public virtual ICollection<int> ParticipantsToRemove { get; set; }
        public virtual ICollection<PlanDatePair<int>> PlansToAdd { get; set; }
        public virtual ICollection<int> PlansToRemove { get; set; }

    }
}
