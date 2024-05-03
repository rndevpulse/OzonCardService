using ClosedXML.Excel;
using CsvHelper.Configuration.Attributes;
using OzonCard.Excel.DataSets.Abstract;

namespace OzonCard.Excel.DataSets.ProgramsReport;

public record ItemProgramReportTable
{
    [Name("Имя гостя"), Total(Label = "Сотрудников")] public string Name { get; init; } = "";
    [Name("Номер карты гостя"), Total(Function = XLTotalsRowFunction.Count)] public string Card { get; init; } = "";
    [Name("Список категорий гостя")] public string Categories { get; init; } = "";
    [Name("Табельный номер гостя")] public string TabNumber { get; init; } = "";
    [Name("Должность"), Total(Label = "Количество обедов")] public string Position { get; init; } = "";
    [Name("Оплачено заказов"), Total(Function = XLTotalsRowFunction.Sum)] public decimal PaidOrders { get; set; }
}