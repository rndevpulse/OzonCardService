using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class ItemTransactionsSummaryTable
{
  
    
    [Name("ФИО")]
    public string Name { get; set; }
    [Name("Количество дней питания")]
    public double CountDay { get; set; }
    [Name("Категории")]
    public string Categories { get; set; }
    [Name("Подразделение")]
    public string Division { get; set; }
}