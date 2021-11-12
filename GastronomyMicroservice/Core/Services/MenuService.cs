using System.Collections.Generic;
using System.Linq;
using Authentication;
using AutoMapper;
using GastronomyMicroservice.Core.Exceptions;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly ILogger<MenuService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IHeaderContextService _headerContextService;

        public MenuService(ILogger<MenuService> logger, MicroserviceContext context, IMapper mapper, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public int Create(int enterpriseId, MenuCoreDto<int> dto)
        {
            var model = _mapper.Map<MenuCoreDto<int>, Menu>(dto);
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.Menus.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int enterpriseId, int menuId)
        {
            var model = new Menu() { Id = menuId, EspId = enterpriseId };

            _context.Menus.Attach(model);
            _context.Menus.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int enterpriseId)
        {
            var dtos = _context.Menus
                .AsNoTracking()
                .Where(m => m.EspId == enterpriseId)
                .Select(m => new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Description
                })
                .AsEnumerable()
                .OrderBy(mx => mx.Name);

            if (dtos is null || dtos.Count() == 0)
            {
                throw new NotFoundException($"NOT FOUND any menu");
            }

            return dtos;
        }

        public object GetById(int enterpriseId, int menuId)
        {
            var dto = _context.Menus
                .AsNoTracking()
                .Include(m => m.DishsToMenus)
                    .ThenInclude(dtm => dtm.Dish)
                .Where(m => m.EspId == enterpriseId && m.Id == menuId)
                .Select(d => new
                {
                    MenuId = d.Id, d.Code, d.Name, d.Description,
                    Value = d.DishsToMenus.Select(dtm => new
                    {
                        dtm.Meal, dtm.DishId, dtm.Dish.Name, dtm.Dish.Description,
                        Ingredients = dtm.Dish.Ingredients.Select(i => new
                        {
                            i.ProductId, i.Product.Name, i.Product.Code, i.ValueOfUse, i.Product.Unit
                        }),
                        Allergens = dtm.Dish.Ingredients.SelectMany(i => i.Product.AllergensToProducts.Select(atp => new
                        {
                            Id = atp.AllergenId, atp.Allergen.Code, atp.Allergen.Name, atp.Allergen.Description
                        }))
                    })
                }).ToList().GroupBy(dx => new {dx.MenuId, dx.Code, dx.Name, dx.Description}).Select(dxg => new
                {
                    dxg.Key,
                    Meals = dxg.SelectMany(g => g.Value.Select(gg => new { gg.Meal, gg.DishId, gg.Name, gg.Description, gg.Ingredients}))
                            .ToList().GroupBy(dxg => new { dxg.Meal }).Select(dxgg => new { 
                                dxgg.Key,
                                Dishes = dxgg.Select(g => new { g.DishId, g.Name, g.Description, g.Ingredients  })
                            }),
                    Ingredients = dxg.SelectMany(g => g.Value.SelectMany(gg => gg.Ingredients)).ToList()
                                 .GroupBy(ggg => new { ggg.ProductId, ggg.Code, ggg.Name, ggg.Unit }).Select(gggx  => new { 
                                    gggx.Key,
                                    Total = gggx.Sum(g => g.ValueOfUse)
                                 }),
                    Allergens = dxg.SelectMany(g => g.Value.SelectMany(gg => gg.Allergens)).ToList().Distinct()
                }).FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Menu with id {menuId} NOT FOUND");
            }

            return dto;
        }

        public object GetDishAllergens(int enterpriseId, int dishId)
        {
            var dtos = _context.Ingredients
                .AsNoTracking()
                .Where(i => i.EspId == enterpriseId && i.DishId == dishId)
                .SelectMany(i => i.Product.AllergensToProducts.Select(atp => new
                    {
                        atp.AllergenId,
                        atp.Allergen.Name,
                        atp.Allergen.Code,
                        atp.Allergen.Description
                    }).AsEnumerable()
                )
                .AsEnumerable()
                .Distinct();

            return dtos;
        }

        public void RemoveDishesFromMenu(int enterpriseId, int menuId, ICollection<int> dishesIds)
        {
            var model = new DishToMenu() { MenuId = menuId, EspId = enterpriseId };

            using (var enumerator = dishesIds.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    model.DishId = enumerator.Current;

                    _context.DishToMenus.Attach(model);
                    _context.DishToMenus.Remove(model);
                }
            }

            _context.SaveChanges();
        }

        public ICollection<int> SetDishesToMenu(int enterpriseId, int menuId, ICollection<DishMealPair<int>> dishMealPairs)
        {
            var model = new DishToMenu() { MenuId = menuId, EspId = enterpriseId };
            
            var responseIds = new HashSet<int>();

            using (var enumerator = dishMealPairs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    model.DishId = enumerator.Current.Dish;
                    model.Meal = enumerator.Current.MealType;

                    _context.DishToMenus.Add(model);
                    responseIds.Add(model.Id);
                }
            }

            _context.SaveChanges();

            return responseIds;
        }
    }
}
