using System.Linq;
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

        public ParticipantService(ILogger<ParticipantService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        public int Create(int espId, int eudId, ParticipantCoreDto<int> dto)
        {
            var model = _mapper.Map<ParticipantCoreDto<int>, Participant>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.Participants.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int espId, int eudId, int id)
        {
            var model = _context.Participants
                .FirstOrDefault(p =>
                    p.Id == id &&
                    p.EspId == espId);

            _context.Participants.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int espId)
        {
            var dtos = _context.Participants
                .AsNoTracking()
                .Where(p => p.EspId == espId)
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int espId, int id)
        {
            var dto = _context.Participants
                .AsNoTracking()
                .Where(p => p.EspId == espId)
                .Where(p => p.Id == id)
                .AsEnumerable();

            return dto;
        }
    }
}
