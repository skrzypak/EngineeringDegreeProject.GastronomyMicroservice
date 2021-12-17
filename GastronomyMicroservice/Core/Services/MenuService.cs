using System.Collections.Generic;
using System.Linq;
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

        public MenuService(ILogger<MenuService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public int Create(int espId, int eudId, MenuCoreDto<int> dto)
        {
            var model = _mapper.Map<MenuCoreDto<int>, Menu>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.Menus.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Update(int espId, int eudId, int menuId, MenuCoreDto<int> dto)
        {
            var model = _context.Menus
                 .Include(m => m.DishsToMenus)
                 .Where(m => m.EspId == espId && m.Id == menuId)
                 .FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Menu with id {menuId} NOT FOUND");
            }

            var item = _mapper.Map<MenuCoreDto<int>, Menu>(dto);

            model.Name = item.Name;
            model.Description = item.Description;
            model.DishsToMenus = item.DishsToMenus;
            model.LastUpdatedEudId = eudId;

            _context.SaveChanges();
        }

        public void Delete(int espId, int eudId, int menuId)
        {
            var model = _context.Menus
                        .FirstOrDefault(m => m.Id == menuId && m.EspId == espId);

            _context.Menus.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int espId)
        {
            var dtos = _context.Menus
                .AsNoTracking()
                .Where(m => m.EspId == espId)
                .Select(m => new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Description
                })
                .AsEnumerable()
                .OrderBy(mx => mx.Name);

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any menu");
            }

            return dtos;
        }

        public object GetById(int espId, int menuId)
        {
            var dto = _context.Menus
                .AsNoTracking()
                .Include(m => m.DishsToMenus)
                    .ThenInclude(dtm => dtm.Dish)
                .Where(m => m.EspId == espId && m.Id == menuId)
                .Select(d => new
                {
                    MenuId = d.Id, d.Code, d.Name, d.Description,
                    Value = d.DishsToMenus.Select(dtm => new
                    {
                        dtm.Meal, menuDishId = dtm.Id, dtm.DishId, dtm.Dish.Name, dtm.Dish.Description,
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
                    Meals = dxg.SelectMany(g => g.Value.Select(gg => new { gg.Meal, gg.menuDishId, gg.DishId, gg.Name, gg.Description, gg.Ingredients}))
                            .ToList().GroupBy(dxg => new { dxg.Meal }).Select(dxgg => new { 
                                dxgg.Key,
                                Dishes = dxgg.Select(g => new { g.menuDishId, g.DishId, g.Name, g.Description, g.Ingredients  })
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

        public object GetDishAllergens(int espId, int dishId)
        {
            var dtos = _context.Ingredients
                .AsNoTracking()
                .Where(i => i.EspId == espId && i.DishId == dishId)
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

        public void RemoveDishesFromMenu(int espId, int eudId, int menuId, ICollection<int> menuDishesIds)
        {
            using (var enumerator = menuDishesIds.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var dtmId = enumerator.Current;

                    var model = _context.DishToMenus
                        .FirstOrDefault(dtm => 
                            dtm.Id == dtmId &&
                            dtm.MenuId == menuId &&
                            dtm.EspId == espId);
                    
                    _context.DishToMenus.Remove(model);
                }
            }

            _context.SaveChanges();
        }

        public ICollection<int> SetDishesToMenu(int espId, int eudId, int menuId, ICollection<DishMealPair<int>> dishMealPairs)
        {
            var model = new DishToMenu() { MenuId = menuId, EspId = espId };
            
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
