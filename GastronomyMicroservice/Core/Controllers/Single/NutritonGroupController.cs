using System;
using Authentication;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/nutrition-groups")]
    public class NutritonGroupController : ControllerBase
    {
        private readonly ILogger<NutritonGroupController> _logger;
        private readonly INutritionGroupService _nutritionGroupService;
        private readonly IHeaderContextService _headerContextService;

        public NutritonGroupController(ILogger<NutritonGroupController> logger, INutritionGroupService nutritionGroupService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _nutritionGroupService = nutritionGroupService;
            _headerContextService = headerContextService;
        }

        [HttpPatch("{nutiGrpId}/plans/{nutriPlsId}")]
        public ActionResult SetNutritionPlan([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromRoute] int nutriPlsId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionGroupService.SetNutritionPlan(espId, eudId, nutiGrpId, nutriPlsId, startDate, endDate);
            return NoContent();
        }

        [HttpPatch("{nutiGrpId}/participants")]
        public ActionResult AddParticipants([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromBody] ICollection<ParticipantDatePair<int>> participants)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionGroupService.AddParticipants(espId, eudId, nutiGrpId, participants);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromQuery] int espId, [FromBody] NutritionGroupCoreDto<int, int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var id = _nutritionGroupService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, nutiGrpId = id }, null);
        }

        [HttpPut("{nutiGrpId}")]
        public async Task<ActionResult> Update([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromBody] NutritionGroupUpdateDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            await _nutritionGroupService.Update(espId, eudId, nutiGrpId, dto);
            return NoContent();
        }

        [HttpDelete("{nutiGrpId}")]
        public ActionResult Delete([FromQuery] int espId, [FromRoute] int nutiGrpId)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionGroupService.Delete(espId, eudId, nutiGrpId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _nutritionGroupService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}")]
        public ActionResult<object> GetById([FromQuery] int espId, [FromRoute] int nutiGrpId)
        {
            var response = _nutritionGroupService.GetById(espId, nutiGrpId);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/plans")]
        public ActionResult<object> GetNutritionPlans([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetNutritionPlans(espId, nutiGrpId, archive);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/participants")]
        public ActionResult<object> GetParticipants([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetParticipants(espId, nutiGrpId, archive);
            return Ok(response);
        }

        [HttpDelete("{nutiGrpId}/plans/{nutiGrpToNutiPlsId}")]
        public ActionResult RemoveNutritionPlan([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromRoute] int nutiGrpToNutiPlsId)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionGroupService.RemoveNutritionPlan(espId, eudId, nutiGrpId, nutiGrpToNutiPlsId);
            return NoContent();
        }

        [HttpDelete("{nutiGrpId}/participants")]
        public ActionResult RemoveParticipants([FromQuery] int espId, [FromRoute] int nutiGrpId, [FromBody] ICollection<int> parcsId)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionGroupService.RemoveParticipants(espId, eudId, nutiGrpId, parcsId);
            return NoContent();
        }
    }
}
