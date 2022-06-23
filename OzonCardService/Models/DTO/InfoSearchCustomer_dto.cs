﻿using OzonCard.BizClient.Models;
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

        internal void SetMetrics(IEnumerable<MetricCustomer> metric_customer)
        {
            Sum = metric_customer.FirstOrDefault(x => x.Metric == CounterMetric.OrdersSum)?.Value ?? 0;
            Orders = (int)(metric_customer.FirstOrDefault(x => x.Metric == CounterMetric.OrdersCount)?.Value ?? 0);
        }
    }
}
