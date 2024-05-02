using ClosedXML.Excel;

namespace OzonCard.Excel.DataSets.Abstract;

public class TotalAttribute : Attribute
{
    public string? Label { get; set; }
    public XLTotalsRowFunction Function { get; set; }
}