using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Dish
{
    public class DishDto<TI, TM> : DishCoreDto<TI>
    {
        public int Id { get; set; }
        public virtual ICollection<TM> Menus { get; set; }
    }
}
