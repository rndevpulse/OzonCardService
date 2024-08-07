﻿using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Customers.Data;

public record CustomersTaskProgress : NamedProgress<CustomersTaskProgress>
{
    public int CountAll { get; set; }
    public int CountNew { get; set; }
    public int CountFail { get; set; }
    public int CountBalance { get; set; }
    public int CountCategory { get; set; }
    public int CountProgram { get; set; }

    public override void Report(CustomersTaskProgress value)
    {
        CountAll = value.CountAll;
        CountNew = value.CountNew;
        CountFail = value.CountFail;
        CountBalance = value.CountBalance;
        CountCategory = value.CountCategory;
        CountProgram = value.CountProgram;
    }

}