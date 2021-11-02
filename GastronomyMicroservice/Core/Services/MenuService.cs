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

        public int Create(MenuCoreDto<int> dto)
        {
            var model = _mapper.Map<MenuCoreDto<int>, Menu>(dto);

            _context.Menus.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int menuId)
        {
            var model = new Menu() { Id = menuId };

            _context.Menus.Attach(model);
            _context.Menus.Remove(model);
            _context.SaveChanges();
        }

        public object Get()
        {
            var dtos = _context.Menus
                .AsNoTracking()
                .Select(m => new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Description
                })
                .ToHashSet()
                .OrderBy(mx => mx.Name);

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any menu");
            }

            return dtos;
        }

        public object GetById(int menuId)
        {
            var dto = _context.Menus
                .AsNoTracking()
                .Include(m => m.DishsToMenus)
                    .ThenInclude(dtm => dtm.Dish)
                .Where(m => m.Id == menuId)
                .Select(m => new
                {
                    m.Id,
                    m.Code,
                    m.Name,
                    m.Description,
                    Dishes = m.DishsToMenus.Select(dtm => new
                    {
                        dtm.Id,
                        dtm.DishId,
                        dtm.Meal,
                        dtm.Dish.Name,
                        dtm.Dish.Description,
                        Allergens = dtm.Dish.Ingredients.SelectMany(i => i.Product.AllergensToProducts.Select(atp => new
                        {
                            atp.AllergenId,
                            atp.Allergen.Code,
                            atp.Allergen.Name,
                            atp.Allergen.Description
                        }))
                        .AsEnumerable()
                        //.Distinct()

                    })
                    .AsEnumerable()
                    //.GroupBy(dtmx => dtmx.Meal)
                    //.ToList()

                 })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Menu with id {menuId} NOT FOUND");
            }

            return dto;
        }

        public object GetMenuAllergens(int menuId)
        {
            var dtos = _context.DishToMenus
                .AsNoTracking()
                .Include(dtm => dtm.Dish)
                    .ThenInclude(d => d.Ingredients)
                .Where(dtm => dtm.MenuId == menuId)
                .SelectMany(dtm => dtm.Dish.Ingredients.SelectMany(i => i.Product.AllergensToProducts.Select(atp => new
                    {
                        atp.AllergenId,
                        atp.Allergen.Name,
                        atp.Allergen.Code,
                        atp.Allergen.Description
                    })
                    .AsEnumerable()
                    .Distinct()
                 ))
                .AsEnumerable()
                .Distinct();

            return dtos;
        }

        public object GetDishAllergens(int dishId)
        {
            var dtos = _context.Ingredients
                .AsNoTracking()
                .Where(i => i.DishId == dishId)
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

        public object GetMenuDishes(int menuId)
        {
            var dtos = _context.DishToMenus
                .AsNoTracking()
                .Include(dtm => dtm.Dish)
                    .ThenInclude(d => d.Ingredients)
                .Where(dtm => dtm.MenuId == menuId)
                .Select(dtm => new
                {
                    dtm.Id,
                    dtm.DishId,
                    dtm.Meal,
                    dtm.Dish.Name,
                    dtm.Dish.Description,
                    Allergens = dtm.Dish.Ingredients.Select(i => i.Product.AllergensToProducts.Select(atp => new
                    {
                        atp.AllergenId,
                        atp.Allergen.Code,
                        atp.Allergen.Name,
                        atp.Allergen.Description
                    }))

                })
                .AsEnumerable()
                .GroupBy(dtmx => dtmx.Meal)
                .ToList();
                
            return dtos;
        }

        public void RemoveDishesFromMenu(int menuId, ICollection<int> dishesIds)
        {
            var model = new DishToMenu() { MenuId = menuId };

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

        public ICollection<int> SetDishesToMenu(int menuId, ICollection<int> dishesIds)
        {
            var model = new DishToMenu() { MenuId = menuId };
            var responseIds = new HashSet<int>();

            using (var enumerator = dishesIds.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    model.DishId = enumerator.Current;

                    _context.DishToMenus.Add(model);
                    responseIds.Add(model.Id);
                }
            }

            _context.SaveChanges();

            return responseIds;
        }
    }
}
