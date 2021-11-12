using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class Ingredient : IEntity
    {
        public int Id { get; set; }
        public float ValueOfUse { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
