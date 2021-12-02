using System.Collections.Generic;
using System.Linq;
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

        public void CreateDishIngredients(int espId, int eudId, int dishId, ICollection<IngredientCoreDto> ingredients)
        {
            var model = new Ingredient() { DishId = dishId, EspId = espId };

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

        public int Create(int espId, int eudId, DishCoreDto<IngredientCoreDto> dto)
        {
            var model = _mapper.Map<DishCoreDto<IngredientCoreDto>, Dish>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.Dishes.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int espId, int eudId, int dishId)
        {
            var model = _context.Dishes
                .FirstOrDefault(d => d.Id == dishId && d.EspId == espId);

            _context.Dishes.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteDishIngredients(int espId, int eudId, int dishId, ICollection<int> ingredientsIds)
        {
            using(var enumerator = ingredientsIds.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    var ingId = enumerator.Current;

                    var model = _context.Ingredients
                        .FirstOrDefault(i => i.Id == ingId && i.DishId == dishId && i.EspId == espId);

                    _context.Ingredients.Remove(model);
                }
            }

            _context.SaveChanges();
        }

        public object GetDishAllergens(int espId, int dishId)
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Include(d => d.Ingredients)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.AllergensToProducts)
                .Where(d => d.EspId == espId &&  d.Id == dishId)
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

        public object GetById(int espId, int dishId)
        {
            var dto = _context.Dishes
                 .AsNoTracking()
                 .Include(d => d.Ingredients)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.AllergensToProducts)
                 .Where(d => d.EspId == espId && d.Id == dishId)
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
                         i.ValueOfUse,
                         i.Product.Unit
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
                 .FirstOrDefault();

            if(dto is null)
            {
                throw new NotFoundException($"Dish with id {dishId} NOT FOUND");
            }

            return dto;
        }

        public object Get(int espId)
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Where(d => d.EspId == espId)
                .Select(d => new
                    {
                        d.Id,
                        d.Name,
                        d.Description,
                    })
                .AsEnumerable();

            return dtos;
        }

        public object GetDishIngredients(int espId, int dishId)
        {
            var dtos = _context.Dishes
                .AsNoTracking()
                .Include(d => d.Ingredients)
                .Where(d => d.EspId == espId && d.Id == dishId)
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
