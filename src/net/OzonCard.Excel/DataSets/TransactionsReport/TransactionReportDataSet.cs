using System.Data;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class TransactionReportDataSet(
    IEnumerable<ItemTransactionsReportTable> transactions,
    IEnumerable<ItemTransactionsSummaryTable> transactionsSummary
) : BaseDataSet
{

    private readonly ITable<ItemTransactionsReportTable> _transactions = 
        new Table<ItemTransactionsReportTable>(transactions);
    private readonly ITable<ItemTransactionsSummaryTable> _transactionsSummary = 
        new Table<ItemTransactionsSummaryTable>(transactionsSummary);

    public override DataSet GetDataSet()
    {
        var ds = new DataSet();
        ds.Tables.Add(ToDataTable(_transactions.ToList(), "Отчет"));
        ds.Tables.Add(ToDataTable(_transactionsSummary.ToList(), "Сводный"));
        return ds;
    }

    public override IEnumerable<TableTotalRow> TotalsRowByTable(string table)
    {
        return table switch
        {
            "Отчет" => _transactions.GetTotalsRow(),
            "Сводный" => _transactionsSummary.GetTotalsRow(),
            _ => ArraySegment<TableTotalRow>.Empty
        };
    }
}


