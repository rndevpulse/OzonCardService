using System;

namespace OzonCardService.Models.View
{
    public class CustomersUpload_vm
    {
        public Guid OrganizationId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid FileReportId { get; set; }
        public double Balance { get; set; }

    }
}
