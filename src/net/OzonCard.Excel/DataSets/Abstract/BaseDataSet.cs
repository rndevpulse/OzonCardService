using System.Data;
using System.Reflection;
using CsvHelper.Configuration.Attributes;

namespace OzonCard.Excel.DataSets.Abstract;

public abstract class BaseDataSet
{
    public abstract DataSet GetDataSet();
    public abstract IEnumerable<TableTotalRow> TotalsRowByTable(string table);
    
    protected DataTable ToDataTable<T>(IList<T> list, string nameTable)
    {
        var elementType = typeof(T).GetProperties().Select(x => 
            new KeyValuePair<PropertyInfo, string?>(
                x,
                x.GetCustomAttribute<NameAttribute>(true)?.Names.First()
                ))
        .ToList();
        elementType.RemoveAll(x => x.Value == null);
        var t = new DataTable(nameTable);

        var row = t.NewRow();
        //add a column to table for each public property on T
        foreach (var propInfo in elementType)
        {
            //Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
            t.Columns.Add(propInfo.Key.Name, typeof(object)); //ColType);
            row[propInfo.Key.Name] = propInfo.Value;
        }

        t.Rows.Add(row);

        //go through each property on T and add each value to the table
        foreach (T item in list)
        {
            row = t.NewRow();
            foreach (var propInfo in elementType)
                row[propInfo.Key.Name] = propInfo.Key.GetValue(item, null) ?? DBNull.Value;

            t.Rows.Add(row);
        }

        var n = 13;
        if (list.Count < n)
            for (var i = 0; i < n; i++)
                t.Rows.Add(t.NewRow());
        
        return t;
    }
    
    
    
    
}