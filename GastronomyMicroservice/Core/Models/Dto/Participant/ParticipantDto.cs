using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Participant
{
    public class ParticipantDto<TNG> : ParticipantCoreDto<TNG>
    {
        public int Id { get; set; }
    }
}
