using OzonCard.BizClient.Models;
using System;
using System.Collections.Generic;

namespace OzonCardService.Models.DTO
{
    public class InfoSearchCustomer_dto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public string TabNumber { get; set; }
        public string Organization { get; set; }
        public double Balance { get; set; }
        public DateTime? TimeUpdateBalance { get; set; }
        public double Sum { get; set; }
        public int Orders { get; set; }
        public IEnumerable<string> Categories { get; set; }


        internal void SetMetrics(ReportCN report)
        {
            if (report == null) return;
            Sum = report?.payFromWalletSum ?? 0;
            Orders = (int)(report?.paidOrdersCount ?? 0);

        }
    }
}
