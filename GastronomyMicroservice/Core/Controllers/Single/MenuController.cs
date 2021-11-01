using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("[controller]")]
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
        public ActionResult CreateMenu([FromBody] MenuCoreDto<int> dto)
        {
            var id = _menuService.CreateMenu(dto);
            return CreatedAtAction(nameof(GetById), new { menuId = id }, null);
        }

        [HttpDelete("{menuId}")]
        public ActionResult DeleteMenu([FromRoute] int menuId)
        {
            _menuService.DeleteMenu(menuId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> GetMenus()
        {
            var response = _menuService.GetMenus();
            return Ok(response);
        }

        [HttpGet("{menuId}")]
        public ActionResult<object> GetById([FromRoute] int menuId)
        {
            var response = _menuService.GetById(menuId);
            return Ok(response);
        }

        [HttpGet("{menuId}/allergen")]
        public ActionResult<object> GetMenuAllergens([FromRoute] int menuId)
        {
            var response = _menuService.GetMenuAllergens(menuId);
            return Ok(response);
        }

        [HttpGet("{menuId}/dish")]
        public ActionResult<object> GetMenuDishes([FromRoute] int menuId)
        {
            var response = _menuService.GetMenuDishes(menuId);
            return Ok(response);
        }

        [HttpDelete("{menuId}/dish")]
        public ActionResult RemoveDishesFromMenu([FromRoute] int menuId, [FromBody] ICollection<int> dishesIds)
        {
            _menuService.RemoveDishesFromMenu(menuId, dishesIds);
            return NoContent();
        }

        [HttpPost("{menuId}/dish")]
        public ActionResult SetDishesToMenu([FromRoute] int menuId, [FromBody] ICollection<int> dishesIds)
        {
            var ids = _menuService.SetDishesToMenu(menuId, dishesIds);
            return NoContent();
        }
    }
}
