using GastronomyMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GastronomyMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/msv")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly MicroserviceContext _context;

        public MicroserviceController(ILogger<MicroserviceController> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("list/day/participants")]
        public ActionResult<object> GetParticipantsForDate(
            [FromQuery] int espId,
            [FromQuery] DateTime queryDate
        )
        {
            var dto = _context.NutritionGroups
                .AsNoTracking()
                .Include(ng => ng.NutritionsGroupsToParticipants)
                    .ThenInclude(ngtp => ngtp.Participant)
                .Where(ng => ng.EspId == espId)
                .Select(ng => new
                {
                    nutritonGroupId = ng.Id,
                    nutritonGroupName = ng.Name,
                    participants = ng.NutritionsGroupsToParticipants
                    .Where(ngtp => ngtp.StartDate.Date <= queryDate.Date && (ngtp.EndDate == null || ngtp.EndDate.Value.Date >= queryDate.Date))
                    .Select(ngtp => new
                    {
                        ngtp.ParticipantId,
                        ngtp.Participant.FirstName,
                        ngtp.Participant.LastName,
                        ngtp.Participant.FullName,
                        ngtp.Participant.Description,
                    }).ToList()
                })
                .ToList()
                .Where(ngx => ngx.participants.Count > 0)
                .GroupBy(ngx => new { ngx.nutritonGroupId, ngx.nutritonGroupName }).Select(ngxg => new
                {
                    ngxg.Key,
                    Participants = ngxg.Select(g => g.participants.Distinct()
                        .OrderBy(ngtpo => ngtpo.LastName)
                        .ThenBy(ngtpo => ngtpo.FirstName)
                    )
                })
                .OrderBy(ngx => ngx.Key.nutritonGroupName)
                .ToList();

            return Ok(dto);
        }

        [HttpGet("list/day/group-plan-menu")]
        public ActionResult<object> GetCurrentState(
            [FromQuery] int espId,
            [FromQuery] DateTime queryDate
        )
        {
            var dto = _context.NutritionGroups
                .AsNoTracking()
                .Include(ng => ng.NutritionsGroupsToParticipants)
                    .ThenInclude(ngtp => ngtp.Participant)
                .Include(ng => ng.NutritionsGroupsToNutritionsPlans)
                    .ThenInclude(ngtnp => ngtnp.NutritionPlan)
                        .ThenInclude(np => np.MenusToNutritonsPlans)
                            .ThenInclude(npm => npm.Menu)
                                .ThenInclude(m => m.DishsToMenus)
                                    .ThenInclude(dtm => dtm.Dish)
                                        .ThenInclude(d => d.Ingredients)
                                            .ThenInclude(i => i.Product)
                                                .ThenInclude(p => p.AllergensToProducts)
                                                    .ThenInclude(atp => atp.Allergen)
                .Where(ng => ng.EspId == espId)
                .Select(ng => new
                {
                    ng.Id,
                    ng.Name,
                    CurrentPlan = ng.NutritionsGroupsToNutritionsPlans
                        .Where(ngtp => ngtp.StartDate.Date <= queryDate.Date && ngtp.EndDate.Date >= queryDate.Date)
                        .Select(ngtnp => new
                        {
                            ngtnp.NutritionPlanId,
                            ngtnp.NutritionPlan.Name,
                            CurrentMenu = ngtnp.NutritionPlan.MenusToNutritonsPlans
                                .Select(mtnp => new
                                {
                                    mtnp.Id,
                                    mtnp.MenuId,
                                    mtnp.Menu.Name,
                                    mtnp.Order
                                })
                                .Where(mtnp => (mtnp.Order - 1) == (queryDate.Date - ngtnp.StartDate.Date).Days % ngtnp.NutritionPlan.MenusToNutritonsPlans.Count)
                                .ToList()
                        }).FirstOrDefault(),
                    //CurrentParticipants = ng.NutritionsGroupsToParticipants
                    //    .Where(ngtp => ngtp.StartDate.Date <= queryDate.Date && (ngtp.EndDate == null || ngtp.EndDate.Value.Date >= queryDate.Date))
                    //    .Select(ngtp => new
                    //    {
                    //        ngtp.ParticipantId,
                    //        ngtp.Participant.FirstName,
                    //        ngtp.Participant.LastName
                    //    }).OrderBy(ngtpo => ngtpo.LastName).ThenBy(ngtpo => ngtpo.FirstName).ToList()
                }).OrderBy(ngx => ngx.Name).ToList();

            return Ok(dto);
        }
    }
}
