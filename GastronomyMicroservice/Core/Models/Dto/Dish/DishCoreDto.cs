using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Dish
{
    public class DishCoreDto<TI>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TI> Ingredients { get; set; }

    }
}
