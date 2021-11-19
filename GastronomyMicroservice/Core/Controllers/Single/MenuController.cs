using Authentication;
using System.Collections.Generic;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/gastronomy/1.0.0/menus")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menuService;
        private readonly IHeaderContextService _headerContextService;

        public MenuController(ILogger<MenuController> logger, IMenuService menuService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _menuService = menuService;
            _headerContextService = headerContextService;
        }

        [HttpPost]
        public ActionResult Create([FromQuery] int espId, [FromBody] MenuCoreDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var id = _menuService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, menuId = id }, null);
        }

        [HttpDelete("{menuId}")]
        public ActionResult Delete([FromQuery] int espId, [FromRoute] int menuId)
        {
            int eudId = _headerContextService.GetEudId();
            _menuService.Delete(espId, eudId, menuId);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _menuService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{menuId}")]
        public ActionResult<object> GetById([FromQuery] int espId, [FromRoute] int menuId)
        {
            var response = _menuService.GetById(espId, menuId);
            return Ok(response);
        }

        [HttpDelete("{menuId}/dishes")]
        public ActionResult RemoveDishesFromMenu([FromQuery] int espId, [FromRoute] int menuId, [FromQuery] ICollection<int> menuDishesIds)
        {
            int eudId = _headerContextService.GetEudId();
            _menuService.RemoveDishesFromMenu(espId, eudId, menuId, menuDishesIds);
            return NoContent();
        }

        [HttpPost("{menuId}/dishes")]
        public ActionResult SetDishesToMenu([FromQuery] int espId, [FromRoute] int menuId, [FromBody] ICollection<DishMealPair<int>> dishMealPairs)
        {
            int eudId = _headerContextService.GetEudId();
            var ids = _menuService.SetDishesToMenu(espId, eudId, menuId, dishMealPairs);
            return Ok(ids);
        }
    }
}
