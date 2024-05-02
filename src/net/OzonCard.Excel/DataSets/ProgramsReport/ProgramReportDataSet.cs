using System.Data;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.ProgramsReport;

public class ProgramReportDataSet(
    IEnumerable<ItemProgramReportTable> report
) : BaseDataSet
{
    public override DataSet GetDataSet()
    {
        var ds = new DataSet();
        ds.Tables.Add(ToDataTable(report.ToList(), "Отчет"));
        return ds;
    }
}