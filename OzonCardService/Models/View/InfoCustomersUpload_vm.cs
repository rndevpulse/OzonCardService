using System;

namespace OzonCardService.Models.View
{
    public class InfoCustomersUpload_vm
    {
        public Guid OrganizationId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CorporateNutritionId { get; set; }
        public string FileReport { get; set; }
        public double Balance { get; set; }
        public AdvancedOptions_vm Options { get; set; }

    }
}
