using System;
using System.Collections.Generic;
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
        private readonly IMenuService _menuService;

        public NutritionPlanService(ILogger<NutritionPlanService> logger, MicroserviceContext context, IMapper mapper, IMenuService menuService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _menuService = menuService;
        }

        public int AddMenu(int nutiPlsId, int menuId, DateTime targetDate)
        {
            var mtnp = _context.MenusToNutritonPlans
                .AsNoTracking()
                .Where(mtnp => mtnp.NutritionPlanId == nutiPlsId)
                .FirstOrDefault();

            if (mtnp is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            var model = new MenuToNutritonPlan()
            {
                NutritionPlanId = nutiPlsId,
                MenuId = menuId,
                TargetDate = targetDate.Date
            };

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

            var info = _context.NutritionPlans
                .AsNoTracking()
                .Where(np => np.Id == nutiPlsId)
                .Select(np => new
                {
                    np.Id,
                    np.Code,
                    np.Name,
                    np.Description,
                    Menus = np.MenusToNutritonsPlans.Select(m => new { m.Id, TargetDate = m.TargetDate.Date }).ToList()
                })
                .FirstOrDefault();

            if (info is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            ICollection<object> menus = new List<object>();

            foreach (var menuInfo in info.Menus)
            {
                var item = _menuService.GetById(menuInfo.Id);
                menus.Add(new { menuInfo.TargetDate, item });
            }

            var dto = new {
                Key = new {
                    info.Id,
                    info.Code,
                    info.Name,
                    info.Description
                },
                Value = menus
            };

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
            _context.MenusToNutritonPlans.Remove(model);

            _context.SaveChanges();
        }
    }
}
