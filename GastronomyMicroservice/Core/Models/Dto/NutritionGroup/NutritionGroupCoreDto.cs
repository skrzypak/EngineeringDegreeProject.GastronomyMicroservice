﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Models.Dto.NutritionGroup
{
    public class NutritionGroupCoreDto<TP, TNP>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TP> Participants { get; set; }
        public virtual ICollection<TNP> Plans { get; set; }
    }
}
