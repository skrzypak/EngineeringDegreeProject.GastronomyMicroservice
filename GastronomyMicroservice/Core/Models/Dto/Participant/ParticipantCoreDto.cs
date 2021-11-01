using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Participant
{
    public class ParticipantCoreDto<TNG>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public virtual ICollection<TNG> NutritionGroups { get; set; } = new List<TNG>();
    }
}
