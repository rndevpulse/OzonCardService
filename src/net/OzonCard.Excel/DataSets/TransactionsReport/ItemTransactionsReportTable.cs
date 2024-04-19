using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class ItemTransactionsReportTable
{
    [Name("Дата")]
    public string Date { get; set; }
    [Name("Время")]
    public string Time { get; set; }
    [Name("Таб. №")]
    public string TabNumber { get; set; }
    [Name("ФИО")]
    public string Name { get; set; }
    [Name("Подразделение")]
    public string Division { get; set; }
    [Name("Категории")]
    public string Categories { get; set; }
    [Name("Прием пищи")]
    public string Eating { get; set; }

    [Ignore]
    public string СardNumbers { get; set; }
}