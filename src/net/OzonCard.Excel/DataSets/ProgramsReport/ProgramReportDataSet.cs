using System.Data;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.ProgramsReport;

public class ProgramReportDataSet(
    IEnumerable<ItemProgramReportTable> report
) : BaseDataSet
{
    
    private readonly ITable<ItemProgramReportTable> _report = 
        new Table<ItemProgramReportTable>(report);
    public override DataSet GetDataSet()
    {
        var ds = new DataSet();
        ds.Tables.Add(ToDataTable(_report.ToList(), "Отчет"));
        return ds;
    }

    public override IEnumerable<TableTotalRow> TotalsRowByTable(string table)
    {
        return table switch
        {
            "Отчет" => _report.GetTotalsRow(),
            _ => ArraySegment<TableTotalRow>.Empty
        };
    }
}