using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Services
{
    public class NutritionGroupService : INutritionGroupService
    {
        private readonly ILogger<NutritionGroupService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public NutritionGroupService(ILogger<NutritionGroupService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public void SetNutritionPlan(int espId, int eudId, int nutiGrpId, int nutriPlsId, DateTime startDate, DateTime endDate)
        {
            var model = new NutritionGroupToNutritionPlan()
            {
                NutritionGroupId = nutiGrpId,
                NutritionPlanId = nutriPlsId,
                StartDate = startDate,
                EndDate = endDate,
                EspId = espId,
                CreatedEudId = eudId
            };

            _context.NutritionsGroupsToNutritionsPlans.Add(model);
            _context.SaveChanges();
        }

        public void AddParticipant(int espId, int eudId, int nutiGrpId, ICollection<int> parcsIds)
        {
            var models = new List<NutritionGroupToParticipant>();

            var time = DateTime.Now;

            foreach (var participantId in parcsIds)
            {
                models.Add(new NutritionGroupToParticipant() { 
                    NutritionGroupId = nutiGrpId,
                    ParticipantId = participantId,
                    StartDate = time,
                    EspId = espId,
                    CreatedEudId = eudId
                });
            }

            _context.NutritionGroupsToParticipants.AddRange(models);
            _context.SaveChanges();
        }

        public int Create(int espId, int eudId, NutritionGroupCoreDto<int, int> dto)
        {
            var model = _mapper.Map<NutritionGroupCoreDto<int, int>, NutritionGroup>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.NutritionGroups.AddRange(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int espId, int eudId, int nutiGrpId)
        {
            var model = _context.NutritionGroups
                       .FirstOrDefault(n => n.Id == nutiGrpId && n.EspId == espId);

            _context.NutritionGroups.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int espId)
        {
            var dtos = _context.NutritionGroups
                .AsNoTracking()
                .Where(n => n.EspId == espId)
                .Select(ng => new
                {
                    ng.Id,
                    ng.Name,
                    ng.Description
                })
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int espId, int nutiGrpId)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionGroups
               .AsNoTracking()
               .Include(ng => ng.NutritionsGroupsToNutritionsPlans)
               .Where(ng => ng.EspId == espId && ng.Id == nutiGrpId)
               .Select(ng => new
               {
                   ng.Id,
                   ng.Name,
                   ng.Description,
                   //Plans = ng.NutritionsGroupsToNutritionsPlans.Select(ngtnp => new
                   //{
                   //    ngtnp.Id,
                   //    ngtnp.NutritionPlanId,
                   //    ngtnp.NutritionPlan.Code,
                   //    ngtnp.NutritionPlan.Name,
                   //    ngtnp.NutritionPlan.Description,
                   //    ngtnp.StartDate,
                   //    ngtnp.EndDate
                   //})
                   //.AsEnumerable()
                   //.Where(px => px.EndDate.Date >= date)
                   //.OrderByDescending(px => px.StartDate).ThenBy(px => px.EndDate)
                   //.Take(5),
                   Participants = ng.NutritionsGroupsToParticipants.Select(ngtp => new { 
                        ngtp.Participant.Id,
                        ngtp.Participant.FirstName,
                        ngtp.Participant.LastName,
                        ngtp.Participant.FullName,
                        ngtp.StartDate,
                        ngtp.EndDate
                   })
                   .AsEnumerable()
                   .Where(ngtpx => ngtpx.EndDate == null)
                   .OrderByDescending(px => px.LastName).ThenBy(px => px.FirstName)
                   .Take(5)
               })
               .FirstOrDefault();

            return dtos;
        }

        public void RemoveNutritionPlan(int espId, int eudId, int nutiGrpId, int nutiGrpToNutiPlsId)
        {

            var model = _context.NutritionsGroupsToNutritionsPlans
                       .FirstOrDefault(n => n.Id == nutiGrpToNutiPlsId && n.NutritionGroupId == nutiGrpId && n.EspId == espId);

            _context.NutritionsGroupsToNutritionsPlans.Remove(model);
            _context.SaveChanges();
        }

        public void RemoveParticipants(int espId, int eudId, int nutiGrpId, ICollection<int> parcsId)
        {
            var models = _context.NutritionGroupsToParticipants
                .Where(ngtp => ngtp.EspId == espId && parcsId.Contains(ngtp.ParticipantId))
                .ToList();

            var time = DateTime.Now;

            models.ForEach(m => m.EndDate = time);

            _context.SaveChanges();
        }

        public object GetNutritionPlans(int espId, int nutiGrpId, bool archive)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionsGroupsToNutritionsPlans
               .AsNoTracking()
               .Include(ngtnp => ngtnp.NutritionPlan)
               .Where(ngtnp => ngtnp.EspId == espId && ngtnp.NutritionGroupId == nutiGrpId)
               .Select(ngtnp => new
               {
                   ngtnp.NutritionPlan.Id,
                   ngtnp.NutritionPlan.Code,
                   ngtnp.NutritionPlan.Name,
                   ngtnp.NutritionPlan.Description,
                   ngtnp.StartDate,
                   ngtnp.EndDate
               })
               .AsEnumerable()
               .Where(px => px.EndDate.Date >= date)
               .OrderByDescending(px => px.StartDate).ThenBy(px => px.EndDate);

            return dtos;
        }

        public object GetParticipants(int espId, int nutiGrpId, bool archive)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionGroupsToParticipants
               .AsNoTracking()
               .Include(ngtp => ngtp.Participant)
               .Where(ngtp => ngtp.EspId == espId && ngtp.NutritionGroupId == nutiGrpId)
               .Select(ngtp => new
               {
                   ngtp.ParticipantId,
                   ngtp.Participant.FirstName,
                   ngtp.Participant.LastName,
                   ngtp.Participant.FullName,
                   ngtp.Participant.Description,
                   ngtp.StartDate,
                   ngtp.EndDate
               })
               .AsEnumerable()
               .Where(px => px.EndDate == null)
               .OrderByDescending(px => px.LastName).ThenBy(px => px.FirstName);

            return dtos;
        }
    }
}
