using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/{enterpriseId}/menus")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menuService;

        public MenuController(ILogger<MenuController> logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int enterpriseId, [FromBody] MenuCoreDto<int> dto)
        {
            var id = _menuService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, menuId = id }, null);
        }

        [HttpDelete("{menuId}")]
        public ActionResult Delete([FromRoute] int enterpriseId, [FromRoute] int menuId)
        {
            _menuService.Delete(enterpriseId, menuId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _menuService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{menuId}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId, [FromRoute] int menuId)
        {
            var response = _menuService.GetById(enterpriseId, menuId);
            return Ok(response);
        }

        [HttpDelete("{menuId}/dishes")]
        public ActionResult RemoveDishesFromMenu([FromRoute] int enterpriseId, [FromRoute] int menuId, [FromBody] ICollection<int> dishesIds)
        {
            _menuService.RemoveDishesFromMenu(enterpriseId, menuId, dishesIds);
            return NoContent();
        }

        [HttpPost("{menuId}/dishes")]
        public ActionResult SetDishesToMenu([FromRoute] int enterpriseId, [FromRoute] int menuId, [FromBody] ICollection<DishMealPair<int>> dishMealPairs)
        {
            var ids = _menuService.SetDishesToMenu(enterpriseId, menuId, dishMealPairs);
            return NoContent();
        }
    }
}
