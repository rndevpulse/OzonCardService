using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using OzonCard.Excel.Models;

namespace OzonCard.Excel;



public class ExcelReader(ILogger<ExcelReader> logger) : IExcelReader
{

    private Customer? ReadRow(IXLRow row, int nameCol, int? tabNumberCol, int? positionCol, int cardCol)
    {
        var card = ReadCardCellValue(row.Cell(cardCol).Value.ToString());
        if (card == "")
            return null;

        return new Customer()
        {
            Name = (row.Cell(nameCol).Value.ToString() ?? "").Replace("  ", " ").Trim(),
            Card = card,
            TabNumber = tabNumberCol == null ? "" : row.Cell((int)tabNumberCol).Value.ToString() ?? "",
            Position = positionCol == null ? "" : row.Cell((int)positionCol).Value.ToString() ?? "",
            Division = ""
        };
    }

    private  Customer? ReadRow(DataRow row, int nameCol, int? tabNumberCol, int? positionCol, int cardCol)
    {
        var card = ReadCardCellValue(row[cardCol].ToString());
        if (card == "")
            return null;

        return new Customer()
        {
            Name = (row[nameCol].ToString() ?? "").Replace("  ", " ").Trim(),
            Card = card,
            TabNumber = tabNumberCol == null ? "" : row[(int)tabNumberCol].ToString() ?? "",
            Position = positionCol == null ? "" : row[(int)positionCol].ToString() ?? "",
            Division = ""
        };
    }

    string ReadCardCellValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(value);
        string card;
        if (matches.Count > 1)
        {
            card = "000".Remove(0, matches[0].Length) + matches[0];
            card += "00000".Remove(0, matches[1].Length) + matches[1];
            return card;
        }

        card = string.Concat(matches.Select(x => x.Value));
        if (card == "" || card.Length > 8)
            return card;
        card = "00000000".Remove(0, card.Length) + card;
        return card;
    }

    void SelDataColumns(int useColumns, int offset, out int nameCol, out int? tabNumberCol, out int? positionCol,
        out int cardCol)
    {
        nameCol = offset;
        tabNumberCol = null;
        positionCol = null;
        cardCol = 2 + offset;
        switch (useColumns)
        {
            case 3:
                break;
            case 5:
                tabNumberCol = offset;
                nameCol = 1 + offset;
                cardCol = 3 + offset;
                positionCol = 4 + offset;
                break;
            case 9:
                positionCol = 1 + offset;
                nameCol = 2 + offset;
                tabNumberCol = 3 + offset;
                cardCol = 7 + offset;
                break;
            default:
                var ex =
                    $"Колличество столбцов ({useColumns}) не соответствует заданному для парсинга количеству (3:5:9)";
                Console.WriteLine("Error {0}", ex);
                throw new ArgumentOutOfRangeException(ex);
        }
    }

    /// <summary>
    /// Read as ExcelDataReader
    /// </summary>
    /// <returns></returns>
    IEnumerable<Customer> ReadFromXls(string file)
    {
        var clientList = new List<Customer>();
        using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
        {
            var reader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration
            {
                UseColumnDataType = false,
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = false
                }
            };
            var dataSet = reader.AsDataSet(conf).Tables[0];
            //нумерация колонок от 0 (A = 0)
            //установка зависимостей используемых столбцов
            SelDataColumns(dataSet.Columns.Count, 0,
                out var nameCol, out var tabNumberCol, out var positionCol, out var cardCol);
            foreach (DataRow row in dataSet.Rows)
            {
                var customer = ReadRow(row, nameCol, tabNumberCol, positionCol, cardCol);
                if (customer == null || clientList.Contains(customer))
                    continue;
                clientList.Add(customer);
            }
        }

        return clientList.ToArray();
    }

    /// <summary>
    /// Read as ClosedXML
    /// </summary>
    /// <returns></returns>
    IEnumerable<Customer> ReadFromXlsx(string file)
    {
        var clientList = new List<Customer>();
        using (var workbook = new XLWorkbook(file))
        {
            var dataSet = workbook.Worksheet(1);
            if (dataSet == null)
                return clientList;
            //нумерация колонок от 1 (A = 1)
            //установка зависимостей используемых столбцов
            SelDataColumns(dataSet.ColumnsUsed().Count(), 1,
                out var nameCol, out var tabNumberCol, out var positionCol, out var cardCol);
            foreach (var row in dataSet.RowsUsed())
            {
                var customer = ReadRow(row, nameCol, tabNumberCol, positionCol, cardCol);
                if (customer == null || clientList.Contains(customer))
                    continue;
                clientList.Add(customer);
            }
        }
        return clientList.ToArray();
    }

    public IEnumerable<Customer> GetCustomers(string file)
    {
        try
        {
            logger.LogInformation("Read file {file}", file);
            return ReadFromXls(file).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail read document '{file}'", file);
            throw new Exception(ex.Message);
        }
    }

    public void CreateWorkbook<T>(string filePath, IList<T> list, string? title = null, bool totalsRow = true)
    {
        var dataset = ToDataSet<T>(list);
        CreateWorkbook(filePath, dataset, title, totalsRow);
    }

    private void CreateWorkbook(string filePath, DataSet dataset, string? title = null, bool totalsRow = true)
    {
        try
        {
            if (dataset.Tables.Count == 0)
                throw new ArgumentException("DataSet needs to have at least one DataTable", nameof(dataset));
            var workbook = new XLWorkbook();
            foreach (DataTable dt in dataset.Tables)
            {
                var worksheet = workbook.Worksheets.Add(dt.TableName);

                if (title != null)
                {
                    worksheet.Cell(1, 1).Value = title;
                    worksheet.Range(1, 1, 1, 5).Merge();
                }

                var range = worksheet.Range(2, 1, dt.Rows.Count + 1, dt.Columns.Count);
                var table = range.CreateTable();
                table.Theme = XLTableTheme.TableStyleLight8;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Add column header
                    //worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);
                    for (int j = 0; j < dt.Rows.Count; j++)
                        //Если нулевые значения, заменяем на пустые строки
                        worksheet.Cell(j + 2, i + 1).Value = dt.Rows[j][i] == DBNull.Value ? "" : dt.Rows[j][i];
                }

                if (totalsRow)
                {
                    table.ShowTotalsRow = true;
                    table.Field(0).TotalsRowLabel = "Сотрудников";
                    table.Field(1).TotalsRowFunction = XLTotalsRowFunction.Count;
                    table.Column(2).SetDataType(XLDataType.Text);
                    table.Column(2).Style.NumberFormat.SetFormat("@");
                    table.Field(dt.Columns.Count - 5).TotalsRowLabel = "Количество обедов";
                    table.Field(dt.Columns.Count - 1).TotalsRowFunction = XLTotalsRowFunction.Sum;
                    table.Field(dt.Columns.Count - 3).TotalsRowFunction = XLTotalsRowFunction.Sum;
                }

                worksheet.Columns().AdjustToContents();

            }

            logger.LogInformation("Save file report to {0}", filePath);
            workbook.SaveAs(filePath);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error on save report");
        }
    }

    private static DataSet ToDataSet<T>(ICollection<T> list)
    {
        var elementType = typeof(T).GetProperties().Select(x => new KeyValuePair<PropertyInfo, string?>(
            x,
            (x.GetCustomAttributes(true)?.FirstOrDefault() as CsvHelper.Configuration.Attributes.NameAttribute)?.Names
            ?.First()
        )).ToList();
        elementType.RemoveAll(x => x.Value == null);
        var ds = new DataSet();
        var t = new DataTable("Отчет");
        ds.Tables.Add(t);

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
        return ds;
    }
}

