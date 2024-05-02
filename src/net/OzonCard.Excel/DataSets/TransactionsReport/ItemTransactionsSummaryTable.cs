using ClosedXML.Excel;
using CsvHelper.Configuration.Attributes;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.TransactionsReport;

public class ItemTransactionsSummaryTable
{
    [Name("ФИО"), Total(Label="Всего дотаций")] public string Name { get; init; } = "";
    [Name("Количество дней питания"), Total(Function = XLTotalsRowFunction.Sum)] public int CountDay { get; init; }
    [Name("Категории")] public string Categories { get; init; } = "";
    [Name("Подразделение")] public string Division { get; init; } = "";
}

