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

        public int AddMenu(int espId, int eudId, int nutiPlsId, int menuId, uint order)
        {
            var np = _context.NutritionPlans
                .AsNoTracking()
                .Where(n => n.EspId == espId && n.Id == nutiPlsId)
                .FirstOrDefault();

            if (np is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            var m = _context.Menus
                .AsNoTracking()
                .Where(m => m.EspId == espId && m.Id == menuId)
                .FirstOrDefault();

            if (m is null)
            {
                throw new NotFoundException($"Menu with id {menuId} NOT FOUND");
            }

            var model = new MenuToNutritonPlan()
            {
                NutritionPlanId = nutiPlsId,
                MenuId = menuId,
                Order = order,
                EspId = espId,
                CreatedEudId = eudId
            };

            _context.MenusToNutritonPlans.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public int Create(int espId, int eudId, NutritionPlanCoreDto<int> dto)
        {
            var model = _mapper.Map<NutritionPlanCoreDto<int>, NutritionPlan>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.NutritionPlans.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Update(int espId, int eudId, int nutiPlsId, NutritionPlanCoreDto<int> dto)
        {
            var model = _context.NutritionPlans
                .Include(np => np.MenusToNutritonsPlans)
                .Where(np => np.EspId == espId && np.Id == nutiPlsId)
                .FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            var item = _mapper.Map<NutritionPlanCoreDto<int>, NutritionPlan>(dto);

            model.Code = item.Code;
            model.Name = item.Name;
            model.Description = item.Description;
            model.MenusToNutritonsPlans = item.MenusToNutritonsPlans;
            model.LastUpdatedEudId = eudId;

            _context.SaveChanges();
        }

        public void Delete(int espId, int eudId, int nutiPlsId)
        {
            var model = _context.NutritionPlans
                       .FirstOrDefault(n => n.Id == nutiPlsId &&  n.EspId == espId);

            _context.NutritionPlans.Remove(model);
            _context.SaveChanges();
        }

        public object Get(int espId)
        {
            var dtos = _context.NutritionPlans
                .AsNoTracking()
                .Where(np => np.EspId == espId)
                .Select(np => new
                {
                    np.Id,
                    np.Code,
                    np.Name,
                    np.Description
                })
                .ToList();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any nutriton plan");
            }

            return dtos;
        }

        public object GetById(int espId, int nutiPlsId)
        {

            var dto = _context.NutritionPlans
                .AsNoTracking()
                .Where(np => np.EspId == espId && np.Id == nutiPlsId)
                .Select(np => new
                {
                    np.Id,
                    np.Code,
                    np.Name,
                    np.Description,
                    Menus = np.MenusToNutritonsPlans.Select(m => new { m.Id, m.MenuId, m.Order, m.Menu.Code, m.Menu.Name, m.Menu.Description })
                    .OrderBy(mx => mx.Order)
                    .ToList()
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Nutriton plan with id {nutiPlsId} NOT FOUND");
            }

            return dto;
        }

        public void RemoveMenu(int espId, int eudId, int nutiPlsId, int menuToPlsId)
        {    
            var model = _context.MenusToNutritonPlans
                .FirstOrDefault(mtm => 
                    mtm.Id == nutiPlsId &&
                    mtm.NutritionPlanId == nutiPlsId &&
                    mtm.EspId == espId);

            _context.MenusToNutritonPlans.Remove(model);

            _context.SaveChanges();
        }
    }
}
