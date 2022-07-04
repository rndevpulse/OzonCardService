using Serilog;
using System.Data;
using System.Reflection;
using ClosedXML.Excel;

namespace OzonCard.Excel
{
    public class ExcelManager
    {
        static private readonly ILogger log = Log.ForContext(typeof(ExcelManager));
        string originalFileName = string.Empty;
        const int Columns = 3;
        int offset;
        public ExcelManager(string file)
        {
            originalFileName = file;
            offset = 0;
        }


        public IEnumerable<ShortCustomerInfo_excel> GetClients()
        {
            try
            {
                var clientList = new List<ShortCustomerInfo_excel>();
                using (var workbook = new XLWorkbook(originalFileName))
                {
                    var dataSet = workbook.Worksheet(1);
                    if (dataSet == null)
                        return clientList;
                    switch (dataSet.ColumnsUsed().Count())
                    {
                        case Columns:
                            break;
                        case Columns + 1:
                            offset = 1;
                            break;
                        case Columns + 2:
                            offset = 1;
                            break;
                        default:
                            var ex = $"Колличество столбцов ({dataSet.ColumnCount()}) не соответствует заданному для парсинга количеству ({Columns})";
                            log.Error(ex,"Error");
                            throw new ArgumentOutOfRangeException(ex);
                    }
                    foreach (var row in dataSet.RowsUsed())
                    {
                        var c = row.Cell(offset + 3);
                        string card = string.Empty;
                        if (row.Cell(offset + 3).Value.ToString()?.Contains("/") == true)
                        {
                            string[] num = (row.Cell(offset + 3).Value.ToString() ?? "").Replace(" ", "").Split('/');
                            num[0] = "000".Remove(0, num[0].Count()) + num[0];
                            num[1] = "00000".Remove(0, num[1].Count()) + num[1];
                            card = num[0] + num[1];
                        }
                        else
                        {
                            try
                            {
                                if ((row.Cell(offset + 3).Value.ToString() ?? "").Trim() == "")
                                    continue;
                                card = "00000000".Remove(0,
                                    row.Cell(offset + 3).Value.ToString()?.Count() ?? 3) + row.Cell(offset + 3).Value.ToString();
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                card = "";
                            }

                        }
                        if (card == "" || clientList
                            .Select(data => data.Card)
                            .Contains(card))
                            continue;
                        if (offset == 1)
                            clientList.Add(new ShortCustomerInfo_excel()
                            {
                                Name = (row.Cell(offset).Value.ToString() ?? "").Replace("  ", " ").Trim(),
                                Card = card,
                                TabNumber = row.Cell(1).Value.ToString() ?? "",
                                Position = row.Cell(dataSet.ColumnsUsed().Count()).Value.ToString() ?? ""
                            });
                        else
                            clientList.Add(new ShortCustomerInfo_excel()
                            {
                                Name = row.Cell(1).Value.ToString() ?? "",
                                Card = card
                            });
                    }

                }
                return clientList.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        public static void CreateWorkbook<T>(String filePath, IList<T> list, string title = null)
        {
            var dataset = ToDataSet<T>(list);
            CreateWorkbook(filePath, dataset, title);
        }
        public static void CreateWorkbook(String filePath, DataSet dataset, string title = null)
        {
            try
            {

                if (dataset.Tables.Count == 0)
                    throw new ArgumentException("DataSet needs to have at least one DataTable", "dataset");

                var workbook = new XLWorkbook();
                foreach (DataTable dt in dataset.Tables)
                {
                    var worksheet = workbook.Worksheets.Add("Отчет");

                    if (title != null)
                    {
                        worksheet.Cell(1, 1).Value = title;
                    }
                    var range = worksheet.Range(2, 1, dt.Rows.Count + 1, dt.Columns.Count);
                    var table = range.CreateTable();
                    table.Theme = XLTableTheme.TableStyleLight15;
                    table.ShowTotalsRow = true;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        // Add column header
                        //worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                        // Populate row data
                        for (int j = 0; j < dt.Rows.Count; j++)
                            //Если нулевые значения, заменяем на пустые строки
                            worksheet.Cell(j + 2, i + 1).Value = dt.Rows[j][i] == DBNull.Value ? "" : dt.Rows[j][i];
                    }
                    table.Field(0).TotalsRowLabel = "Сотрудников";
                    table.Field(1).TotalsRowFunction = XLTotalsRowFunction.Count;
                    
                    table.Field(dt.Columns.Count - 5).TotalsRowLabel = "Количество обедов";
                    table.Field(dt.Columns.Count - 1).TotalsRowFunction = XLTotalsRowFunction.Sum;
                    table.Field(dt.Columns.Count - 3).TotalsRowFunction = XLTotalsRowFunction.Sum;
                }

                log.Information("Save file report to {0}", filePath);
                workbook.SaveAs(filePath);
            }

            catch (Exception ex)
            {
                log.Error(ex, "Error on save report");
            }
        }

        static DataSet ToDataSet<T>(IList<T> list)
        {
            var elementType = typeof(T).GetProperties().Select(x => new KeyValuePair<PropertyInfo, string?>(
                x,
                (x.GetCustomAttributes(true)?.FirstOrDefault() as CsvHelper.Configuration.Attributes.NameAttribute)?.Names?.First()
                )).ToList();
            elementType.RemoveAll(x => x.Value == null);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            DataRow row = t.NewRow();
            //add a column to table for each public property on T
            foreach (var propInfo in elementType)
            {
                //Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                t.Columns.Add(propInfo.Key.Name, typeof(object));//ColType);
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
}
