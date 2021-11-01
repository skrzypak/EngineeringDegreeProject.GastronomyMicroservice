using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GastronomyMicroservice.Core.Exceptions;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Services
{
    public class DishService : IDishService
    {
        private readonly ILogger<DishService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public DishService(ILogger<DishService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public void AddDishIngredients(int dishId, ICollection<IngredientCoreDto> ingredients)
        {
            var model = new Ingredient() { DishId = dishId };

            using (var enumerator = ingredients.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    model.ProductId = item.ProductId;
                    model.ValueOfUse = item.ValueOfUse;

                    _context.Ingredients.Add(model);
                }
            }

            _context.SaveChanges();
        }

        public int CreateDish(DishCoreDto<IngredientCoreDto> dto)
        {
            var model = _mapper.Map<DishCoreDto<IngredientCoreDto>, Dish>(dto);

            _context.Dishes.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void DeleteDish(int dishId)
        {
            var model = new Dish() { Id = dishId };

            _context.Dishes.Attach(model);
            _context.Dishes.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteDishIngredients(int dishId, ICollection<int> ingredientsIds)
        {
            var model = new Ingredient() { DishId = dishId };

            using(var enumerator = ingredientsIds.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    model.Id = enumerator.Current;
                    _context.Ingredients.Attach(model);
                    _context.Ingredients.Remove(model);
                }
            }

            _context.SaveChanges();
        }

        public object GetDishAllergens(int dishId)
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Include(d => d.Ingredients)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.AllergensToProducts)
                .Where(d => d.Id == dishId)
                .SelectMany(d => d.Ingredients.SelectMany(i => i.Product.AllergensToProducts
                    .Select(atp => new
                    {
                        atp.AllergenId,
                        atp.Allergen.Code,
                        atp.Allergen.Name,
                        atp.Allergen.Description
                    })
                ))
                .AsEnumerable();

            return dtos;
        }

        public object GetDishById(int dishId)
        {
            var dto = _context.Dishes
                 .AsNoTracking()
                 .Include(d => d.Ingredients)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.AllergensToProducts)
                 .Where(d => d.Id == dishId)
                 .Select(d => new
                 {
                     d.Id,
                     d.Name,
                     d.Description,
                     Ingredients = d.Ingredients.Select(i => new
                     {
                         i.Id,
                         i.ProductId,
                         i.Product.Name,
                         i.Product.Code,
                         i.ValueOfUse
                     }).AsEnumerable(),
                     Allergens = d.Ingredients.AsEnumerable().SelectMany(i => i.Product.AllergensToProducts
                        .Select(atp => new
                        {
                            atp.AllergenId,
                            atp.Allergen.Code,
                            atp.Allergen.Name,
                            atp.Allergen.Description
                        })
                    ).AsEnumerable()
                 })
                 .AsEnumerable();

            if(dto is null)
            {
                throw new NotFoundException($"Dish with id {dishId} NOT FOUND");
            }

            return dto;
        }

        public object GetDishes()
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Select(d => new
                    {
                        d.Id,
                        d.Name,
                        d.Description,
                    })
                .AsEnumerable();

            return dtos;
        }

        public object GetDishIngredients(int dishId)
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Include(d => d.Ingredients)
                .Where(d => d.Id == dishId)
                .SelectMany(d => d.Ingredients.Select(i => new {
                        i.Product.Id,
                        i.Product.Name,
                        i.Product.Code,
                        i.ValueOfUse
                    })
                )
                .AsEnumerable();

            return dtos;
        }
    }
}
