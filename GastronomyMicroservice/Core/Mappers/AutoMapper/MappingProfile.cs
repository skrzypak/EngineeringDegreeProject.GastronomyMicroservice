using System.Collections.Generic;
using AutoMapper;
using GastronomyMicroservice.Core.Fluent.Entities;
using GastronomyMicroservice.Core.Models.Dto.Participant;

namespace GastronomyMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
                            });;
                        }
                    }
                });
        }
    }
}
