using System.Collections.Generic;

namespace GastronomyMicroservice.Core.Models.Dto.Dish
{
    public class DishDto<TI, TM> : DishCoreDto<TI>
    {
        public int Id { get; set; }
        public virtual ICollection<TM> Menus { get; set; }
    }
}
