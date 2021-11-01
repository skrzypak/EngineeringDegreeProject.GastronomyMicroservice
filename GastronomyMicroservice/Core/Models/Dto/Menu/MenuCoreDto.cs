using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.Menu
{
    public class MenuCoreDto<TD>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TD> Dishes { get; set; }
    }
}
