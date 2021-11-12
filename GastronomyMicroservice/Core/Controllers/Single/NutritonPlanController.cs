using System;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/{enterpriseId}/nutrition-plans")]
    public class NutritonPlanController : ControllerBase
    {
        private readonly ILogger<NutritonPlanController> _logger;
        private readonly INutritionPlanService _nutritionPlanService;

        public NutritonPlanController(ILogger<NutritonPlanController> logger, INutritionPlanService nutritionPlanService)
        {
            _logger = logger;
            _nutritionPlanService = nutritionPlanService;
        }

        [HttpPatch("{nutiPlsId}/menus/{menuId}")]
        public ActionResult AddMenu([FromRoute] int enterpriseId, [FromRoute] int nutiPlsId, [FromRoute] int menuId, [FromQuery] DateTime targetDate)
        {
            _nutritionPlanService.AddMenu(enterpriseId, nutiPlsId, menuId, targetDate);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int enterpriseId, [FromBody] NutritionPlanCoreDto<int> dto)
        {
            var id = _nutritionPlanService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, nutiPlsId = id }, null);
        }

        [HttpDelete("{nutiPlsId}")]
        public ActionResult Delete([FromRoute] int enterpriseId, [FromRoute] int nutiPlsId)
        {
            _nutritionPlanService.Delete(enterpriseId, nutiPlsId);
            return NoContent();
        }

        [HttpGet]
        public object Get([FromRoute] int enterpriseId)
        {
            var response = _nutritionPlanService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}")]
        public object GetById([FromRoute] int enterpriseId, [FromRoute] int nutiPlsId)
        {
            var response = _nutritionPlanService.GetById(enterpriseId, nutiPlsId);
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}/menus/{menuToPlsId}")]
        public ActionResult RemoveMenu([FromRoute] int enterpriseId, [FromRoute] int nutiPlsId, [FromRoute] int menuToPlsId)
        {
            _nutritionPlanService.RemoveMenu(enterpriseId, nutiPlsId, menuToPlsId);
            return NoContent();
        }
    }
}
