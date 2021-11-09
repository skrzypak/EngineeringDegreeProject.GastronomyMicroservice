using System;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/nutrition-groups")]
    public class NutritonGroupController : ControllerBase
    {
        private readonly ILogger<NutritonGroupController> _logger;
        private readonly INutritionGroupService _nutritionGroupService;

        public NutritonGroupController(ILogger<NutritonGroupController> logger, INutritionGroupService nutritionGroupService)
        {
            _logger = logger;
            _nutritionGroupService = nutritionGroupService;
        }

        [HttpPatch("{nutiGrpId}/plans/{nutriPlsId}")]
        public ActionResult SetNutritionPlan([FromRoute] int nutiGrpId, [FromRoute] int nutriPlsId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            _nutritionGroupService.SetNutritionPlan(nutiGrpId, nutriPlsId, startDate, endDate);
            return NoContent();
        }

        [HttpPatch("{nutiGrpId}")]
        public ActionResult AddParticipants([FromRoute] int nutiGrpId, [FromBody] ICollection<int> parcsIds)
        {
            _nutritionGroupService.AddParticipant(nutiGrpId, parcsIds);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromBody] NutritionGroupCoreDto<int> dto)
        {
            var id = _nutritionGroupService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { nutiGrpId = id }, null);
        }

        [HttpDelete("{nutiGrpId}")]
        public ActionResult Delete([FromRoute] int nutiGrpId)
        {
            _nutritionGroupService.Delete(nutiGrpId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> Get()
        {
            var response = _nutritionGroupService.Get();
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}")]
        public ActionResult<object> GetById([FromRoute] int nutiGrpId)
        {
            var response = _nutritionGroupService.GetById(nutiGrpId);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/plans")]
        public ActionResult<object> GetNutritionPlans([FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetNutritionPlans(nutiGrpId, archive);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/participants")]
        public ActionResult<object> GetParticipants([FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetParticipants(nutiGrpId, archive);
            return Ok(response);
        }

        [HttpDelete("{nutiGrpId}/plans/{nutiPlsId}")]
        public ActionResult RemoveNutritionPlan([FromRoute] int nutiGrpId, [FromRoute] int nutiPlsId)
        {
            _nutritionGroupService.RemoveNutritionPlan(nutiGrpId, nutiPlsId);
            return NoContent();
        }

        [HttpDelete("{nutiGrpId}/participants")]
        public ActionResult RemoveParticipants([FromRoute] int nutiGrpId, [FromBody] ICollection<int> parcsId)
        {
            _nutritionGroupService.RemoveParticipants(nutiGrpId, parcsId);
            return NoContent();
        }
    }
}
