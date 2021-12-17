using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GastronomyMicroservice.Core.Exceptions;
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

        public void AddParticipants(int espId, int eudId, int nutiGrpId, ICollection<ParticipantDatePair<int>> participants)
        {
            var models = new List<NutritionGroupToParticipant>();

            foreach (var participant in participants)
            {
                models.Add(new NutritionGroupToParticipant() { 
                    NutritionGroupId = nutiGrpId,
                    ParticipantId = participant.ParticipantId,
                    StartDate = participant.StartDate,
                    EndDate = participant.EndDate,
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

        public async Task Update(int espId, int eudId, int nutiGrpId, NutritionGroupUpdateDto dto)
        {
            var model = _context.NutritionGroups
                 .Include(n => n.NutritionsGroupsToNutritionsPlans)
                 .Include(n => n.NutritionsGroupsToParticipants)
                 .Where(n => n.EspId == espId && n.Id == nutiGrpId)
                 .FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Nutrtion group with id {nutiGrpId} NOT FOUND");
            }

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        model.Name = dto.Name;
                        model.Description = dto.Description;
                        model.LastUpdatedEudId = eudId;

                        this.AddParticipants(espId, eudId, nutiGrpId, dto.ParticipantsToAdd);

                        this.RemoveParticipants(espId, eudId, nutiGrpId, dto.ParticipantsToRemove);

                        foreach (var item in dto.PlansToAdd)
                        {
                            this.SetNutritionPlan(espId, eudId,nutiGrpId, item.NutritionPlanId, item.StartDate, item.EndDate);
                        }

                        foreach (var item in dto.PlansToRemove)
                        {
                            this.RemoveNutritionPlan(espId, eudId, nutiGrpId, item);
                        }

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }); 
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

            var dto = _context.NutritionGroups
               .AsNoTracking()
               .Include(ng => ng.NutritionsGroupsToNutritionsPlans)
               .Where(ng => ng.EspId == espId && ng.Id == nutiGrpId)
               .Select(ng => new
               {
                   ng.Id,
                   ng.Name,
                   ng.Description,
               })
               .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Nutrtion group with id {nutiGrpId} NOT FOUND");
            }

            object nutritionPlans = null, participants = null;

            try
            {
                nutritionPlans = this.GetNutritionPlans(espId, nutiGrpId, false);
            } catch(Exception) {
                nutritionPlans = new List<object>();
            }

            try
            {
                participants = this.GetParticipants(espId, nutiGrpId, false);
            }
            catch (Exception) {
                participants = new List<object>();
            }

            return new
            {
                dto.Id,
                dto.Name,
                dto.Description,
                nutritionPlans,
                participants,
            };
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
            DateTime time = !archive ? DateTime.Now : DateTime.MinValue;

            var date = time.Date;

            var dtos = _context.NutritionsGroupsToNutritionsPlans
               .AsNoTracking()
               .Include(ngtnp => ngtnp.NutritionPlan)
               .Where(ngtnp => ngtnp.EspId == espId && ngtnp.NutritionGroupId == nutiGrpId)
               .Select(ngtnp => new
               {
                   ngtnp.Id,
                   nutritionPlanId = ngtnp.NutritionPlan.Id,
                   ngtnp.NutritionPlan.Code,
                   ngtnp.NutritionPlan.Name,
                   ngtnp.NutritionPlan.Description,
                   StartDate = ngtnp.StartDate.ToString("dd/MM/yyyy"),
                   EndDate = ngtnp.EndDate.Date.ToString("dd/MM/yyyy"),
                   sdc = ngtnp.StartDate.Date,
                   edc = ngtnp.EndDate.Date
               })
               .AsEnumerable()
               .Where(px => px.edc >= date)
               .OrderByDescending(px => px.sdc).ThenBy(px => px.edc);

            return dtos;
        }

        public object GetParticipants(int espId, int nutiGrpId, bool archive)
        {
            DateTime time = !archive ? DateTime.Now : DateTime.MinValue;

            var date = time.Date;

            var dtos = _context.NutritionGroupsToParticipants
               .AsNoTracking()
               .Include(ngtp => ngtp.Participant)
               .Where(ngtp => ngtp.EspId == espId && ngtp.NutritionGroupId == nutiGrpId)
               .Select(ngtp => new
               {
                   id = ngtp.ParticipantId,
                   ngtp.Participant.FirstName,
                   ngtp.Participant.LastName,
                   ngtp.Participant.FullName,
                   ngtp.Participant.Description,
                   StartDate = ngtp.StartDate.ToString("dd/MM/yyyy"),
                   EndDate = ngtp.EndDate == null ? null : ngtp.EndDate.Value.Date.ToString("dd/MM/yyyy"),
                   sdc = ngtp.StartDate.Date,
                   edc = ngtp.EndDate
               })
               .AsEnumerable()
               .Where(px => (px.edc == null && !archive) || px.edc >= date)
               .OrderByDescending(px => px.LastName).ThenBy(px => px.FirstName);

            return dtos;
        }
    }
}
