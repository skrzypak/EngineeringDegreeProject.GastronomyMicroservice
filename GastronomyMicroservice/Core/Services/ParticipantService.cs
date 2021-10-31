using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public int Create(ParticipantCoreDto<int> dto)
        {
            var model = _mapper.Map<ParticipantCoreDto<int>, Participant>(dto);

            _context.Participants.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int id)
        {
            var model = new Participant() { Id = id };

            _context.Participants.Attach(model);
            _context.Participants.Remove(model);
            _context.SaveChanges();
        }

        public object Get()
        {
            var dtos = _context.Participants
                .AsNoTracking()
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int id)
        {
            var dto = _context.Participants
                .AsNoTracking()
                .Where(p => p.Id == id)
                .AsEnumerable();

            return dto;
        }
    }
}
