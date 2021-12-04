using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionGroup
{
    public class NutritionGroupCoreDto<TP, TNP>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ParticipantDatePair<TP>> Participants { get; set; }
        public virtual ICollection<PlanDatePair<TNP>> Plans { get; set; }
    }

    public class ParticipantDatePair<T>
    {
        public T ParticipantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PlanDatePair<T>
    {
        public T NutritionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
