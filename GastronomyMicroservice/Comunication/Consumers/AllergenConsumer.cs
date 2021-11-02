using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using GastronomyMicroservice.Core.Fluent;
using GastronomyMicroservice.Core.Fluent.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace GastronomyMicroservice.Comunication.Consumers
{
    public class AllergenConsumer : IConsumer<Payload<AllergenPayloadValue>>
    {
        readonly ILogger<AllergenConsumer> _logger;
        private readonly MicroserviceContext _context;

        public AllergenConsumer(ILogger<AllergenConsumer> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<Payload<AllergenPayloadValue>> context)
        {
            _logger.LogInformation("Received Alergen data: {Text}", context.Message.Value);

            switch(context.Message.Type)
            {
                case CRUD.Create:
                {
                    Create(context.Message.Value); 
                    break;
                }
                case CRUD.Update:
                {
                    Update(context.Message.Value);
                    break;
                }
                case CRUD.Delete:
                {
                    Delete(context.Message.Value);
                    break;
                }
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        private void Create(AllergenPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Allergens.Add(model);
        }

        private void Update(AllergenPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Allergens.Update(model);
        }

        private void Delete(AllergenPayloadValue val)
        {
            var model = _context.Allergens.FirstOrDefault(p => p.Id == val.Id);
            _context.Allergens.Remove(model);
        }

        private Allergen MapToModel(AllergenPayloadValue val)
        {
            return new Allergen()
            {
                Id = val.Id,
                Code = val.Code,
                Name = val.Name,
                Description = val.Description
            };
        }


    }
}
