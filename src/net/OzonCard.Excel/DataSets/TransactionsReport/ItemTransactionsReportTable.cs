using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class ItemTransactionsReportTable
{
    [Name("Дата")] public string Date { get; init; } = "";
    [Name("Время")] public string Time { get; init; } = "";
    [Name("Таб. №")] public string TabNumber { get; init; } = "";
    [Name("ФИО")] public string Name { get; init; } = "";
    [Name("Подразделение")] public string Division { get; init; } = "";
    [Name("Должность")] public string Position { get; init; } = "";
    [Name("Категории")] public string Categories { get; init; } = "";
    [Name("Прием пищи")] public string Eating { get; init; } = "";
    
    [Ignore] public DateTime Created { get; init; }
    [Ignore] public string Cards { get; init; } = "";

}