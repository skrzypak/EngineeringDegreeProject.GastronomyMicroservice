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
    [Route("[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IDishService _dishService;

        public DishController(ILogger<DishController> logger, IDishService dishService)
        {
            _logger = logger;
            _dishService = dishService;
        }

        [HttpPatch("{dishId}/ingredient/add")]
        public ActionResult CreateDishIngredients([FromRoute] int dishId, [FromBody] ICollection<IngredientCoreDto> ingredients)
        {
            _dishService.CreateDishIngredients(dishId, ingredients);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<int> Create(DishCoreDto<IngredientCoreDto> dto)
        {
            var id = _dishService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { dishId = id }, null);
        }

        [HttpDelete]
        public ActionResult Delete(int dishId)
        {
            _dishService.Delete(dishId);
            return NoContent();
        }

        [HttpPatch("{dishId}/ingredient/remove")]
        public ActionResult DeleteDishIngredients([FromRoute] int dishId, [FromBody] ICollection<int> ingredientsIds)
        {
            _dishService.DeleteDishIngredients(dishId, ingredientsIds);
            return NoContent();
        }

        [HttpGet("{dishId}/allergen")]
        public ActionResult<object> GetDishAllergens(int dishId)
        {
            var response = _dishService.GetDishAllergens(dishId);
            return Ok(response);
        }

        [HttpGet("{dishId}")]
        public ActionResult<object> GetById(int dishId)
        {
            var response = _dishService.GetById(dishId);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult<object> Get()
        {
            var response = _dishService.Get();
            return Ok(response);
        }

        [HttpGet("{dishId}/ingredient")]
        public ActionResult<object> GetDishIngredients(int dishId)
        {
            var response = _dishService.GetDishIngredients(dishId);
            return Ok(response);
        }

    }
}
