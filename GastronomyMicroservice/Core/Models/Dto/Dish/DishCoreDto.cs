using System.Collections.Generic;

namespace GastronomyMicroservice.Core.Models.Dto.Dish
{
    public class DishCoreDto<TI>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TI> Ingredients { get; set; }

    }
}
