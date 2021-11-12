using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/{enterpriseId}/dishes")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IDishService _dishService;

        public DishController(ILogger<DishController> logger, IDishService dishService)
        {
            _logger = logger;
            _dishService = dishService;
        }

        [HttpPatch("{dishId}/ingredients/add")]
        public ActionResult CreateDishIngredients([FromRoute] int enterpriseId, [FromRoute] int dishId, [FromBody] ICollection<IngredientCoreDto> ingredients)
        {
            _dishService.CreateDishIngredients(enterpriseId, dishId, ingredients);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<int> Create([FromRoute] int enterpriseId, DishCoreDto<IngredientCoreDto> dto)
        {
            var id = _dishService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, dishId = id }, null);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int enterpriseId, int dishId)
        {
            _dishService.Delete(enterpriseId, dishId);
            return NoContent();
        }

        [HttpPatch("{dishId}/ingredients/remove")]
        public ActionResult DeleteDishIngredients([FromRoute] int enterpriseId, [FromRoute] int dishId, [FromBody] ICollection<int> ingredientsIds)
        {
            _dishService.DeleteDishIngredients(enterpriseId, dishId, ingredientsIds);
            return NoContent();
        }

        [HttpGet("{dishId}/allergens")]
        public ActionResult<object> GetDishAllergens([FromRoute] int enterpriseId, int dishId)
        {
            var response = _dishService.GetDishAllergens(enterpriseId, dishId);
            return Ok(response);
        }

        [HttpGet("{dishId}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId, int dishId)
        {
            var response = _dishService.GetById(enterpriseId, dishId);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _dishService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{dishId}/ingredients")]
        public ActionResult<object> GetDishIngredients([FromRoute] int enterpriseId, int dishId)
        {
            var response = _dishService.GetDishIngredients(enterpriseId, dishId);
            return Ok(response);
        }

    }
}
