using System.Data;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class TransactionReportDataSet(
    IEnumerable<ItemTransactionsReportTable> transactions,
    IEnumerable<ItemTransactionsSummaryTable> transactionsSummary
) : BaseDataSet
{


    public override DataSet GetDataSet()
    {
        var ds = new DataSet();
        ds.Tables.Add(ToDataTable(transactions.ToList(), "Отчет"));
        ds.Tables.Add(ToDataTable(transactionsSummary.ToList(), "Сводный"));
        return ds;
    }

}