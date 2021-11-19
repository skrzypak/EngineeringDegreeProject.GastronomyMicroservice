using Authentication;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/dishes")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IDishService _dishService;
        private readonly IHeaderContextService _headerContextService;

        public DishController(ILogger<DishController> logger, IDishService dishService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _dishService = dishService;
            _headerContextService = headerContextService;
        }

        [HttpPatch("{dishId}/ingredients/add")]
        public ActionResult CreateDishIngredients([FromQuery] int espId, [FromRoute] int dishId, [FromBody] ICollection<IngredientCoreDto> ingredients)
        {
            int eudId = _headerContextService.GetEudId();
            _dishService.CreateDishIngredients(espId, eudId, dishId, ingredients);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<int> Create([FromQuery] int espId, DishCoreDto<IngredientCoreDto> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var id = _dishService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, dishId = id }, null);
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery] int espId, int dishId)
        {
            int eudId = _headerContextService.GetEudId();
            _dishService.Delete(espId, eudId, dishId);
            return NoContent();
        }

        [HttpPatch("{dishId}/ingredients/remove")]
        public ActionResult DeleteDishIngredients([FromQuery] int espId, [FromRoute] int dishId, [FromBody] ICollection<int> ingredientsIds)
        {
            int eudId = _headerContextService.GetEudId();
            _dishService.DeleteDishIngredients(espId, eudId, dishId, ingredientsIds);
            return NoContent();
        }

        [HttpGet("{dishId}/allergens")]
        public ActionResult<object> GetDishAllergens([FromQuery] int espId, int dishId)
        {
            var response = _dishService.GetDishAllergens(espId, dishId);
            return Ok(response);
        }

        [HttpGet("{dishId}")]
        public ActionResult<object> GetById([FromQuery] int espId, int dishId)
        {
            var response = _dishService.GetById(espId, dishId);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _dishService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{dishId}/ingredients")]
        public ActionResult<object> GetDishIngredients([FromQuery] int espId, int dishId)
        {
            var response = _dishService.GetDishIngredients(espId, dishId);
            return Ok(response);
        }

    }
}
