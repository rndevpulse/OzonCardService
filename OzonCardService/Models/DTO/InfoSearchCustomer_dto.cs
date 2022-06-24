using OzonCard.BizClient.Models;
using OzonCard.BizClient.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OzonCardService.Models.DTO
{
    public class InfoSearchCustomer_dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public string Organization { get; set; }
        public double Balanse { get; set; }
        public double Sum { get; set; }
        public int Orders { get; set; }
        public IEnumerable<string> Categories { get; set; }


        internal void SetMetrics(ReportCN report)
        {
            Sum = report?.payFromWalletSum ?? 0;
            Orders = (int)(report?.paidOrdersCount ?? 0);

        }
    }
}
