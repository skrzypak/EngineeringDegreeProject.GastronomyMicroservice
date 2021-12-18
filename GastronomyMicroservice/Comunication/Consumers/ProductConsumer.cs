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
    public class ProductConsumer : IConsumer<Payload<ProductPayloadValue>>
    {
        readonly ILogger<ProductConsumer> _logger;
        private readonly MicroserviceContext _context;

        public ProductConsumer(ILogger<ProductConsumer> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<Payload<ProductPayloadValue>> context)
        {
            _logger.LogInformation("Received Product data: {Text}", context.Message.Value);

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

        private void Create(ProductPayloadValue val)
        {
            var model = MapToModel(val);
            model.CreatedEudId = val.EudId;
            _context.Products.Add(model);
        }

        private void Update(ProductPayloadValue val)
        {
            var model = MapToModel(val);
            model.LastUpdatedEudId = val.EudId;
            _context.Products.Update(model);
        }

        private void Delete(ProductPayloadValue val)
        {
            var model = _context.Products.FirstOrDefault(p => p.EspId == val.EspId && p.Id == val.Id);
            _context.Products.Remove(model);
        }

        private Product MapToModel(ProductPayloadValue val)
        {
            var model = new Product()
            {
                Id = val.Id,
                Code = val.Code,
                Name = val.Name,
                Unit = val.Unit,
                Description = val.Description,
                EspId = val.EspId,
                AllergensToProducts = new HashSet<AllergenToProduct>(),
                Calories = val.Calories,
                Proteins = val.Proteins,
                Carbohydrates = val.Carbohydrates,
                Fats = val.Fats
            };

            foreach(var map in val.Allergens)
            {
                if (map.Value == CRUD.Create)
                {
                    model.AllergensToProducts.Add(new AllergenToProduct()
                    {
                        AllergenId = map.Key,
                        ProductId = model.Id,
                        EspId = val.EspId,
                        CreatedEudId = val.EudId
                    });
                }

                if (map.Value == CRUD.Update)
                {
                    model.AllergensToProducts.Add(new AllergenToProduct()
                    {
                        AllergenId = map.Key,
                        ProductId = model.Id,
                        EspId = val.EspId,
                        LastUpdatedEudId = val.EudId
                    }); ;
                }

                if (map.Value == CRUD.Exists)
                {
                    model.AllergensToProducts.Add(new AllergenToProduct()
                    {
                        AllergenId = map.Key,
                        ProductId = model.Id,
                    });
                }
            }

            return model;
        }


    }
}
