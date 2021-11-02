using System.Collections.Generic;
using AutoMapper;
using Comunication.Shared.PayloadValue;
using GastronomyMicroservice.Core.Fluent.Entities;

namespace GastronomyMicroservice.Comunication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductPayloadValue, Product>();
            CreateMap<AllergenPayloadValue, Allergen>();
        }
    }
}
