using OzonCard.BizClient.Models;
using OzonCard.Data.Models;
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

        public string LastVisit { get; set; }
        public int DaysGrant { get; set; }

        public InfoSearchCustomer_dto()
        {
        }

        internal void SetMetrics(ReportCN report)
        {
            if (report == null) return;
            Sum = report?.payFromWalletSum ?? 0;
            Orders = (int)(report?.paidOrdersCount ?? 0);

        }
        internal void SetMetrics(CustomerReport? report)
        {
            if (report == null) return;
            Sum = report?.payFromWalletSum ?? 0;
            Orders = (int)(report?.paidOrdersCount ?? 0);

        }

        public void SetLastVisitDate(DateTime? lastVisit) =>
            LastVisit = lastVisit?.ToString("dd.MM.yyyy HH:mm:ss") ?? string.Empty;

        public void SetCountGrant(int count) 
            => DaysGrant = count;
    }
}
