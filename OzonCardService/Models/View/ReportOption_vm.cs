using System;

namespace OzonCardService.Models.View
{
    public class ReportOption_vm
    {
        public Guid OrganizationId { get; set; }
        public Guid CorporateNutritionId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Title { get; set; }
    }
}
