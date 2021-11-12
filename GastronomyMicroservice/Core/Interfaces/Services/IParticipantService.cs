using GastronomyMicroservice.Core.Models.Dto.Participant;

namespace GastronomyMicroservice.Core.Interfaces.Services
{
    public interface IParticipantService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int id);
        public int Create(int enterpriseId, ParticipantCoreDto<int> dto);
        public void Delete(int enterpriseId, int id);
    }
}
