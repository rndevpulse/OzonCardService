using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.ProgramsReport;

public record ItemProgramReportTable
{
    [Name("Имя гостя")] public string Name { get; init; } = "";
    [Name("Номер карты гостя")] public string Card { get; init; } = "";
    [Name("Список категорий гостя")] public string Categories { get; init; } = "";
    [Name("Табельный номер гостя")] public string TabNumber { get; init; } = "";
    [Name("Подразделение")] public string Division { get; init; } = "";
    [Name("Должность")] public string Position { get; init; } = "";
}