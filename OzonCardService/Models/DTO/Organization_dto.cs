using System;
using System.Collections.Generic;

namespace OzonCardService.Models.DTO
{
    public class Organization_dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Category_dto> Categories { get; set; }
        public IEnumerable<CorporateNutrition_dto> CorporateNutritions { get; set; }

    }
    
    
}
