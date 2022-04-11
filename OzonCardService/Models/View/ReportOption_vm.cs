using System;

namespace OzonCardService.Models.View
{
    public class ReportOption_vm
    {
        public Guid OrganizationId { get; set; }
        public Guid CorporateNutritionId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Title { get; set; }
    }
}
