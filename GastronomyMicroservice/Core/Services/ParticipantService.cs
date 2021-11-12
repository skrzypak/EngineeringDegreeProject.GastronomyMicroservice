using System.Linq;
using Authentication;
using AutoMapper;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Participant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly ILogger<ParticipantService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IHeaderContextService _headerContextService;

        public ParticipantService(ILogger<ParticipantService> logger, MicroserviceContext context, IMapper mapper, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }
        public int Create(int enterpriseId, ParticipantCoreDto<int> dto)
        {
            var model = _mapper.Map<ParticipantCoreDto<int>, Participant>(dto);
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.Participants.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int enterpriseId, int id)
        {
            var model = _context.Participants
                .FirstOrDefault(p =>
                    p.Id == id &&
                    p.EspId == enterpriseId);

            _context.Participants.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int enterpriseId)
        {
            var dtos = _context.Participants
                .AsNoTracking()
                .Where(p => p.EspId == enterpriseId)
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int enterpriseId, int id)
        {
            var dto = _context.Participants
                .AsNoTracking()
                .Where(p => p.EspId == enterpriseId)
                .Where(p => p.Id == id)
                .AsEnumerable();

            return dto;
        }
    }
}
