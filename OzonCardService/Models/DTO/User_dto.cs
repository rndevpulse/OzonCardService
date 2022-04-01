using System;
using System.Collections.Generic;

namespace OzonCardService.Models.DTO
{
    public class User_dto
    {
        public Guid Id { get; set; }
        public string Mail { get; set; }
        public IEnumerable<Organization_dto> Organizations { get; set; }

    }
}
