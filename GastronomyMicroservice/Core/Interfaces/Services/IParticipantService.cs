using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Models.Dto.Participant;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IParticipantService
    {
        public object Get();
        public object GetById(int id);
        public int Create(ParticipantCoreDto<int> dto);
        public void Delete(int id);
    }
}
