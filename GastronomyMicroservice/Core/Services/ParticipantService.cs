using System.Linq;
using AutoMapper;
using GastronomyMicroservice.Core.Exceptions;
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
				.Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Description,
                    p.FullName
                })
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int espId, int id)
        {
            var dto = _context.Participants
                .AsNoTracking()
                .Include(p => p.NutritionsGroupsToParticipants)
                    .ThenInclude(n2p => n2p.NutritionGroup)
                .Where(p => p.EspId == espId)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Description,
                    p.FullName,
                    nutritionGroups = p.NutritionsGroupsToParticipants.Select(n2p => new
                    {
                        id = n2p.NutritionGroupId,
                        n2p.NutritionGroup.Name,
                        n2p.NutritionGroup.Description,
                        startDate = n2p.StartDate,
                        endDate = n2p.EndDate
                    })
                    .OrderByDescending(n2px => n2px.startDate)
                    .ToList()
                })
                .FirstOrDefault();

            if(dto is null)
            {
                throw new NotFoundException($"Participant with ID {id} NOT FOUND in enterprsie with ID {espId}");
            }

            return dto;
        }
    }
}
