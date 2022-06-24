using System;

namespace OzonCardService.Models.View
{
    public class SearchCustomer_vm
    {
        public string Name { get; set; }
        public string Card { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid CorporateNutritionId { get; set; }
    }
}
