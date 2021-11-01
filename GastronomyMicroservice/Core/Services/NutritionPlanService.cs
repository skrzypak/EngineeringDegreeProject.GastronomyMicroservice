using System.Linq;
using AutoMapper;
using GastronomyMicroservice.Core.Exceptions;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Interfaces.Services;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Core.Services
{
    public class NutritionPlanService : INutritionPlanService
    {
        private readonly ILogger<NutritionPlanService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public NutritionPlanService(ILogger<NutritionPlanService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public int AddMenu(int nutiPlsId, int menuId, int orderNumber)
        {
            var mtnp = _context.MenusToNutritonPlans
                .Where(mtnp => mtnp.NutritionPlanId == nutiPlsId)
                .ToList();

            if (mtnp is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            var model = new MenuToNutritonPlan()
            {
                NutritionPlanId = nutiPlsId,
                MenuId = menuId,
                OrderNumber = orderNumber
            };

            if (mtnp.Count <= orderNumber)
            {
                model.OrderNumber = mtnp.Count + 1;
            } else
            {
                for(int i = orderNumber - 1; i < mtnp.Count; i++)
                {
                    mtnp[i].OrderNumber += 1;
                }
            }

            _context.MenusToNutritonPlans.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public int Create(NutritionPlanCoreDto<int> dto)
        {
            var model = _mapper.Map<NutritionPlanCoreDto<int>, NutritionPlan>(dto);

            _context.NutritionPlans.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int nutiPlsId)
        {
            var model = new NutritionPlan() { Id = nutiPlsId };

            _context.NutritionPlans.Attach(model);
            _context.NutritionPlans.Remove(model);
            _context.SaveChanges();
        }

        public object Get()
        {
            var dtos = _context.NutritionPlans
                .AsNoTracking()
                .Select(np => new
                {
                    np.Id,
                    np.Code,
                    np.Name,
                    np.Description
                })
                .FirstOrDefault();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any nutriton plan");
            }

            return dtos;
        }

        public object GetById(int nutiPlsId)
        {
            var dto = _context.NutritionPlans
                .AsNoTracking()
                .Where(np => np.Id == nutiPlsId)
                .Select(np => new
                {
                    np.Id,
                    np.Code,
                    np.Name,
                    np.Description,
                    Menus = np.MenusToNutritonsPlans.Select(mtnp => new
                    {
                        mtnp.Id,
                        mtnp.MenuId,
                        mtnp.Menu.Code,
                        mtnp.Menu.Name,
                        mtnp.Menu.Description,
                        mtnp.OrderNumber,
                        Dishes = mtnp.Menu.DishsToMenus.Select(dtm => new
                        {
                            dtm.Id,
                            dtm.DishId,
                            dtm.Meal,
                            Ingredients = dtm.Dish.Ingredients.Select(i => new
                            {
                                i.Id,
                                i.ProductId,
                                i.ValueOfUse,
                                i.Product.Name,
                                i.Product.Unit
                            }),
                            Allergens = dtm.Dish.Ingredients.Select(i => 
                                i.Product.AllergensToProducts.Select(atp => new {
                                    atp.Id,
                                    atp.AllergenId,
                                    atp.ProductId,
                                    atp.Allergen.Code,
                                    atp.Allergen.Name,
                                    atp.Allergen.Description
                                })
                            ),
                        })
                    })
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            return dto;
        }

        public void RemoveMenu(int nutiPlsId, int menuToPlsId)
        {    
            var model = new MenuToNutritonPlan()
            {
                Id = menuToPlsId,
                NutritionPlanId = nutiPlsId,
            };

            _context.MenusToNutritonPlans.Attach(model);

            var orderNumber = model.OrderNumber;

            _context.MenusToNutritonPlans.Remove(model);

            var mtnp = _context.MenusToNutritonPlans
                .Where(mtnp => mtnp.NutritionPlanId == nutiPlsId)
                .OrderBy(mtnp => mtnp.OrderNumber)
                .ToList();

            for (int i = orderNumber - 1; i < mtnp.Count; i++)
            {
                mtnp[i].OrderNumber -= 1;
            }

            _context.SaveChanges();
        }
    }
}
