using System;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/nutrition-plans")]
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
        public ActionResult AddMenu([FromRoute] int nutiPlsId, [FromRoute] int menuId, [FromQuery] DateTime targetDate)
        {
            _nutritionPlanService.AddMenu(nutiPlsId, menuId, targetDate);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromBody] NutritionPlanCoreDto<int> dto)
        {
            var id = _nutritionPlanService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { nutiPlsId = id }, null);
        }

        [HttpDelete("{nutiPlsId}")]
        public ActionResult Delete([FromRoute] int nutiPlsId)
        {
            _nutritionPlanService.Delete(nutiPlsId);
            return NoContent();
        }

        [HttpGet]
        public object Get()
        {
            var response = _nutritionPlanService.Get();
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}")]
        public object GetById([FromRoute] int nutiPlsId)
        {
            var response = _nutritionPlanService.GetById(nutiPlsId);
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}/menus/{menuToPlsId}")]
        public ActionResult RemoveMenu([FromRoute] int nutiPlsId, [FromRoute] int menuToPlsId)
        {
            _nutritionPlanService.RemoveMenu(nutiPlsId, menuToPlsId);
            return NoContent();
        }
    }
}
