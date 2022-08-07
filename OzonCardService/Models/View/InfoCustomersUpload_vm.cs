using System;
using System.Collections.Generic;

namespace OzonCardService.Models.View
{
    public class InfoCustomersUpload_vm
    {
        public Guid OrganizationId { get; set; }
        public IEnumerable<Guid> CategoriesId { get; set; }
        public Guid CorporateNutritionId { get; set; }
        public string FileReport { get; set; }
        public double Balance { get; set; }
        public AdvancedOptions_vm Options { get; set; }
        public Customer_vm? Customer { get; set; }

    }
}
