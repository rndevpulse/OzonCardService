using System;

namespace OzonCardService.Models.View
{
    public class ChangeCustomerCategory_vm
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid CategoryId { get; set; }
        public bool isRemove { get; set; }
    }
}
