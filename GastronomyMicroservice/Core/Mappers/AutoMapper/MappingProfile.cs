using System.Collections.Generic;
using AutoMapper;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Models.Dto.Dish;
using GastronomyMicroservice.Core.Models.Dto.Ingredient;
using GastronomyMicroservice.Core.Models.Dto.Menu;
using GastronomyMicroservice.Core.Models.Dto.NutritionGroup;
using GastronomyMicroservice.Core.Models.Dto.NutritionPlan;
using GastronomyMicroservice.Core.Models.Dto.Participant;

namespace GastronomyMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IngredientCoreDto, Ingredient>()
                .ForMember(dest => dest.DishId, opt => opt.Ignore());

            CreateMap<DishCoreDto<IngredientCoreDto>, Dish>()
                .ForMember(dest => dest.Ingredients, opt => opt.Ignore())
                 .AfterMap((src, dest) => {
                     dest.Ingredients = new List<Ingredient>();

                     using (var enumerator = src.Ingredients.GetEnumerator())
                     {
                         while (enumerator.MoveNext())
                         {
                             dest.Ingredients.Add(new Ingredient()
                             {
                                 ProductId = enumerator.Current.ProductId,
                                 ValueOfUse = enumerator.Current.ValueOfUse,
                             });
                         }
                     }
                 });

            CreateMap<MenuCoreDto<int>, Menu>()
                .ForMember(dest => dest.DishsToMenus, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.DishsToMenus = new List<DishToMenu>();

                    using (var enumerator = src.Dishes.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            dest.DishsToMenus.Add(new DishToMenu()
                            {
                                DishId = enumerator.Current,
                            });
                        }
                    }
                });

            CreateMap<ParticipantCoreDto<int>, Participant>()
                .ForMember(dest => dest.NutritionsGroupsToParticipants, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.NutritionsGroupsToParticipants = new HashSet<NutritionGroupToParticipant>();

                    using(var enumerator = src.NutritionsGroups.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            dest.NutritionsGroupsToParticipants.Add(new NutritionGroupToParticipant() {
                                NutritionGroupId = enumerator.Current,
                                StartDate = System.DateTime.Now
                            });
                        }
                    }
                });

            CreateMap<NutritionGroupCoreDto, NutritionGroup>()
                .ForMember(dest => dest.NutritionsGroupsToParticipants, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.NutritionsGroupsToParticipants = new HashSet<NutritionGroupToParticipant>();

                    using (var enumerator = src.ParticipantsIds.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            dest.NutritionsGroupsToParticipants.Add(new NutritionGroupToParticipant()
                            {
                                NutritionGroupId = enumerator.Current,
                                StartDate = System.DateTime.Now
                            });
                        }
                    }
                });

            CreateMap<NutritionPlanCoreDto<int>, NutritionPlan>()
                .ForMember(dest => dest.NutritionsGroupsToNutritionsPlans, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.MenusToNutritonsPlans = new HashSet<MenuToNutritonPlan>();

                    using (var enumerator = src.Menus.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            dest.MenusToNutritonsPlans.Add(new MenuToNutritonPlan()
                            {
                                MenuId = enumerator.Current
                            });
                        }
                    }
                });
        }
    }
}
