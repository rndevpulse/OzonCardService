using System.Reflection;
using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.Abstract;

public interface ITable<out T>:IEnumerable<T>
{
    IEnumerable<TableTotalRow> GetTotalsRow();
}

internal class Table<T>(IEnumerable<T> collection) : List<T>(collection), ITable<T>
{
    public IEnumerable<TableTotalRow> GetTotalsRow()
    {
        var props = typeof(T).GetProperties();
        foreach (var property in props)
        {
            var field = property.GetCustomAttribute<NameAttribute>()?.Names.FirstOrDefault();
            if (string.IsNullOrEmpty(field))
                continue;
            var total = property.GetCustomAttribute<TotalAttribute>();
            if (total == null)
                continue;
            yield return new TableTotalRow(
                field,
                total.Label,
                total.Function);
        }
    }
}

