using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;

namespace Comunication.Shared.PayloadValue
{
    public class AllergenPayloadValue : IMessage
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
