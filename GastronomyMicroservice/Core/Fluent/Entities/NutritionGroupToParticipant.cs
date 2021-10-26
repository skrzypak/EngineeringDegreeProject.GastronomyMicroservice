using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class NutritionGroupToParticipant : IEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NutritionGroupId { get; set; }
        public virtual NutritionGroup NutritionGroup { get; set; }
        public int ParticipantId { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
