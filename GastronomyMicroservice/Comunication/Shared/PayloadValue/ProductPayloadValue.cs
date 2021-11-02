using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;
using GastronomyMicroservice.Core.Fluent.Enums;

namespace Comunication.Shared.PayloadValue
{
    public class ProductPayloadValue : IMessage
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UnitType Unit { get; set; }
        public virtual ICollection<int> Allergens { get; set; }
    }
}
