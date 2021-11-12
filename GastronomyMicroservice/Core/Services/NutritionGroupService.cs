using System;
using System.Collections.Generic;
using System.Linq;
using Authentication;
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
        private readonly IHeaderContextService _headerContextService;

        public NutritionGroupService(ILogger<NutritionGroupService> logger, MicroserviceContext context, IMapper mapper, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public void SetNutritionPlan(int enterpriseId, int nutiGrpId, int nutriPlsId, DateTime startDate, DateTime endDate)
        {
            var model = new NutritionGroupToNutritionPlan()
            {
                NutritionGroupId = nutiGrpId,
                NutritionPlanId = nutriPlsId,
                StartDate = startDate,
                EndDate = endDate,
                EspId = enterpriseId,
                CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId)
            };

            _context.NutritionsGroupsToNutritionsPlans.Add(model);
            _context.SaveChanges();
        }

        public void AddParticipant(int enterpriseId, int nutiGrpId, ICollection<int> parcsIds)
        {
            var models = new List<NutritionGroupToParticipant>();

            var time = DateTime.Now;

            foreach (var participantId in parcsIds)
            {
                models.Add(new NutritionGroupToParticipant() { 
                    NutritionGroupId = nutiGrpId,
                    ParticipantId = participantId,
                    StartDate = time,
                    EspId = enterpriseId,
                    CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId)
                });
            }

            _context.NutritionGroupsToParticipants.AddRange(models);
            _context.SaveChanges();
        }

        public int Create(int enterpriseId, NutritionGroupCoreDto<int> dto)
        {
            var model = _mapper.Map<NutritionGroupCoreDto<int>, NutritionGroup>(dto);
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.NutritionGroups.AddRange(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int enterpriseId, int nutiGrpId)
        {
            var model = new NutritionGroup() { Id = nutiGrpId, EspId = enterpriseId };

            _context.NutritionGroups.Attach(model);
            _context.NutritionGroups.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int enterpriseId)
        {
            var dtos = _context.NutritionGroups
                .AsNoTracking()
                .Where(n => n.EspId == enterpriseId)
                .Select(ng => new
                {
                    ng.Id,
                    ng.Name,
                    ng.Description
                })
                .AsEnumerable();

            return dtos;
        }

        public object GetById(int enterpriseId, int nutiGrpId)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionGroups
               .AsNoTracking()
               .Include(ng => ng.NutritionsGroupsToNutritionsPlans)
               .Where(ng => ng.EspId == enterpriseId && ng.Id == nutiGrpId)
               .Select(ng => new
               {
                   ng.Id,
                   ng.Name,
                   ng.Description,
                   Plans = ng.NutritionsGroupsToNutritionsPlans.Select(ngtnp => new { 
                        ngtnp.NutritonPlan.Id,
                        ngtnp.NutritonPlan.Code,
                        ngtnp.NutritonPlan.Description,
                        ngtnp.StartDate,
                        ngtnp.EndDate,
                   })
                   .AsEnumerable()
                   .Where(px => px.EndDate.Date >= date)
                   .OrderByDescending(px => px.StartDate).ThenBy(px => px.EndDate)
                   .Take(5),
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
               .AsEnumerable();

            return dtos;
        }

        public void RemoveNutritionPlan(int enterpriseId, int nutiGrpId, int nutiPlsId)
        {
            var model = new NutritionGroupToNutritionPlan() { NutritionGroupId = nutiGrpId, NutritionPlanId = nutiPlsId, EspId = enterpriseId };

            _context.NutritionsGroupsToNutritionsPlans.Attach(model);
            _context.NutritionsGroupsToNutritionsPlans.Remove(model);
            _context.SaveChanges();
        }

        public void RemoveParticipants(int enterpriseId, int nutiGrpId, ICollection<int> parcsId)
        {
            var models = _context.NutritionGroupsToParticipants
                .Where(ngtp => ngtp.EspId == enterpriseId && parcsId.Contains(ngtp.ParticipantId))
                .ToList();

            var time = DateTime.Now;

            models.ForEach(m => m.EndDate = time);

            _context.SaveChanges();
        }

        public object GetNutritionPlans(int enterpriseId, int nutiGrpId, bool archive)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionsGroupsToNutritionsPlans
               .AsNoTracking()
               .Include(ngtnp => ngtnp.NutritonPlan)
               .Where(ngtnp => ngtnp.EspId == enterpriseId && ngtnp.NutritionGroupId == nutiGrpId)
               .Select(ngtnp => new
               {
                   ngtnp.NutritonPlan.Id,
                   ngtnp.NutritonPlan.Code,
                   ngtnp.NutritonPlan.Name,
                   ngtnp.NutritonPlan.Description,
                   ngtnp.StartDate,
                   ngtnp.EndDate
               })
               .AsEnumerable()
               .Where(px => px.EndDate.Date >= date)
               .OrderByDescending(px => px.StartDate).ThenBy(px => px.EndDate);

            return dtos;
        }

        public object GetParticipants(int enterpriseId, int nutiGrpId, bool archive)
        {
            var time = DateTime.Now;
            var date = time.Date;

            var dtos = _context.NutritionGroupsToParticipants
               .AsNoTracking()
               .Include(ngtp => ngtp.Participant)
               .Where(ngtp => ngtp.EspId == enterpriseId && ngtp.NutritionGroupId == nutiGrpId)
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
