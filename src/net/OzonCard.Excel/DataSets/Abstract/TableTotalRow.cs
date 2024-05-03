using ClosedXML.Excel;

namespace OzonCard.Excel.DataSets.Abstract;

public record TableTotalRow(string Field, string? Label, XLTotalsRowFunction Function);
