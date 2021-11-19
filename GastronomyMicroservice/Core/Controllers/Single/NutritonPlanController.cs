using System;
using Authentication;
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
        private readonly IHeaderContextService _headerContextService;

        public NutritonPlanController(ILogger<NutritonPlanController> logger, INutritionPlanService nutritionPlanService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _nutritionPlanService = nutritionPlanService;
            _headerContextService = headerContextService;
        }

        [HttpPatch("{nutiPlsId}/menus/{menuId}")]
        public ActionResult AddMenu([FromQuery] int espId, [FromRoute] int nutiPlsId, [FromRoute] int menuId, [FromQuery] DateTime targetDate)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionPlanService.AddMenu(espId, eudId, nutiPlsId, menuId, targetDate);
            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromQuery] int espId, [FromBody] NutritionPlanCoreDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var id = _nutritionPlanService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, nutiPlsId = id }, null);
        }

        [HttpDelete("{nutiPlsId}")]
        public ActionResult Delete([FromQuery] int espId, [FromRoute] int nutiPlsId)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionPlanService.Delete(espId, eudId, nutiPlsId);
            return NoContent();
        }

        [HttpGet]
        public object Get([FromQuery] int espId)
        {
            var response = _nutritionPlanService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}")]
        public object GetById([FromQuery] int espId, [FromRoute] int nutiPlsId)
        {
            var response = _nutritionPlanService.GetById(espId, nutiPlsId);
            return Ok(response);
        }

        [HttpGet("{nutiPlsId}/menus/{menuToPlsId}")]
        public ActionResult RemoveMenu([FromQuery] int espId, [FromRoute] int nutiPlsId, [FromRoute] int menuToPlsId)
        {
            int eudId = _headerContextService.GetEudId();
            _nutritionPlanService.RemoveMenu(espId, eudId, nutiPlsId, menuToPlsId);
            return NoContent();
        }
    }
}
