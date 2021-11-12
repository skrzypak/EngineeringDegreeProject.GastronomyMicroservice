using System;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/{enterpriseId}/nutrition-groups")]
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
        public ActionResult SetNutritionPlan([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromRoute] int nutriPlsId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            _nutritionGroupService.SetNutritionPlan(enterpriseId, nutiGrpId, nutriPlsId, startDate, endDate);
            return NoContent();
        }

        [HttpPatch("{nutiGrpId}")]
        public ActionResult AddParticipants([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromBody] ICollection<int> parcsIds)
        {
            _nutritionGroupService.AddParticipant(enterpriseId, nutiGrpId, parcsIds);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int enterpriseId, [FromBody] NutritionGroupCoreDto<int> dto)
        {
            var id = _nutritionGroupService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, nutiGrpId = id }, null);
        }

        [HttpDelete("{nutiGrpId}")]
        public ActionResult Delete([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId)
        {
            _nutritionGroupService.Delete(enterpriseId, nutiGrpId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _nutritionGroupService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId)
        {
            var response = _nutritionGroupService.GetById(enterpriseId, nutiGrpId);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/plans")]
        public ActionResult<object> GetNutritionPlans([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetNutritionPlans(enterpriseId, nutiGrpId, archive);
            return Ok(response);
        }

        [HttpGet("{nutiGrpId}/participants")]
        public ActionResult<object> GetParticipants([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromQuery] bool archive)
        {
            var response = _nutritionGroupService.GetParticipants(enterpriseId, nutiGrpId, archive);
            return Ok(response);
        }

        [HttpDelete("{nutiGrpId}/plans/{nutiPlsId}")]
        public ActionResult RemoveNutritionPlan([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromRoute] int nutiPlsId)
        {
            _nutritionGroupService.RemoveNutritionPlan(enterpriseId, nutiGrpId, nutiPlsId);
            return NoContent();
        }

        [HttpDelete("{nutiGrpId}/participants")]
        public ActionResult RemoveParticipants([FromRoute] int enterpriseId, [FromRoute] int nutiGrpId, [FromBody] ICollection<int> parcsId)
        {
            _nutritionGroupService.RemoveParticipants(enterpriseId, nutiGrpId, parcsId);
            return NoContent();
        }
    }
}
