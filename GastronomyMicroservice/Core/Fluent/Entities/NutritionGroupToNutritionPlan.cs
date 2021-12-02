﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GastronomyMicroservice.Core.Fluent.Entities
{
    public class NutritionGroupToNutritionPlan : IEntity
    {
        public int Id { get; set; }
        public int NutritionGroupId { get; set; }
        public int NutritionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual NutritionGroup NutritionGroup { get; set; }
        public virtual NutritionPlan NutritionPlan { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
