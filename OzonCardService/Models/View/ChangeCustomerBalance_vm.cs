using System;

namespace OzonCardService.Models.View
{
    public class ChangeCustomerBalance_vm
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid CorporateNutritionId { get; set; }
        public bool isIncrement { get; set; }
        public double Balance { get; set; }
    }
}
