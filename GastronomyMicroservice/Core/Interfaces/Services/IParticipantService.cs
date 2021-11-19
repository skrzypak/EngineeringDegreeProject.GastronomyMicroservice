using GastronomyMicroservice.Core.Models.Dto.Participant;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IParticipantService
    {
        public object Get(int espId);
        public object GetById(int espId, int id);
        public int Create(int espId, int eudId, ParticipantCoreDto<int> dto);
        public void Delete(int espId, int eudId, int id);
    }
}
