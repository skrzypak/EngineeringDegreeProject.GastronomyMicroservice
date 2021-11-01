using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Menu
{
    public class MenuDto<TNP, TD> : MenuCoreDto<TD>
    {
        public int Id { get; set; }
        public virtual ICollection<TNP> NutritonsPlans { get; set; }
    }
}
