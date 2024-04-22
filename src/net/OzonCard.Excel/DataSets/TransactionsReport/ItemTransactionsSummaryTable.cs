using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class ItemTransactionsSummaryTable
{
    [Name("ФИО")] public string Name { get; init; } = "";
    [Name("Количество дней питания")] public int CountDay { get; init; }
    [Name("Категории")] public string Categories { get; init; } = "";
    [Name("Подразделение")] public string Division { get; init; } = "";
    [Name("Должность")] public string Position { get; init; } = "";
}