using Serilog;
using System.Data;
using System.Reflection;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace OzonCard.Excel
{
    public class ExcelManager
    {
        static private readonly ILogger log = Log.ForContext(typeof(ExcelManager));
        string originalFileName = string.Empty;

        public ExcelManager(string file)
        {
            originalFileName = file;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        ShortCustomerInfo_excel? ReadRow(IXLRow row, int nameCol, int? TabNumberCol, int? PositionCol, int CardCol)
        {
            string card = ReadCardCellValue(row.Cell(CardCol).Value.ToString());
            if (card == "")
                return null;

            return new ShortCustomerInfo_excel()
            {
                Name = (row.Cell(nameCol).Value.ToString() ?? "").Replace("  ", " ").Trim(),
                Card = card,
                TabNumber = TabNumberCol == null ? "" : row.Cell((int)TabNumberCol).Value.ToString() ?? "",
                Position = PositionCol == null ? "" : row.Cell((int)PositionCol).Value.ToString() ?? ""
            };
        }

        ShortCustomerInfo_excel? ReadRow(DataRow row, int nameCol, int? TabNumberCol, int? PositionCol, int CardCol)
        {
            string card = ReadCardCellValue(row[CardCol].ToString());
            if (card == "")
                return null;

            return new ShortCustomerInfo_excel()
            {
                Name = (row[nameCol].ToString() ?? "").Replace("  ", " ").Trim(),
                Card = card,
                TabNumber = TabNumberCol == null ? "" : row[(int)TabNumberCol].ToString() ?? "",
                Position = PositionCol == null ? "" : row[(int)PositionCol].ToString() ?? ""
            };
        }

        string ReadCardCellValue(string? value)
        {
            if (value == null || value == "")
                return string.Empty;
            Regex regex = new Regex(@"\d+");
            var matches = regex.Matches(value);
            string card = string.Empty;
            if (matches.Count > 1)
            {
                card = "000".Remove(0, matches[0].Length) + matches[0];
                card += "00000".Remove(0, matches[1].Length) + matches[1];
                return card;
            }
            card = string.Concat(matches.Select(x => x.Value));
            if (card == "")
                return card;
            card = "00000000".Remove(0, card.Length) + card;
            return card;
        }
        void SelDataCollumns(int useColumns, int offset, out int NameCol, out int? TabNumberCol, out int? PositionCol, out int CardCol)
        {
            NameCol = offset;
            TabNumberCol = null;
            PositionCol = null;
            CardCol = 2 + offset;
            switch (useColumns)
            {
                case 3:
                    break;
                case 5:
                    TabNumberCol = offset;
                    NameCol = 1 + offset;
                    CardCol = 3 + offset;
                    PositionCol = 4 + offset;
                    break;
                case 9:
                    PositionCol = 1 + offset;
                    NameCol = 2 + offset;
                    TabNumberCol = 3 + offset;
                    CardCol = 7 + offset;
                    break;
                default:
                    var ex = $"Колличество столбцов ({useColumns}) не соответствует заданному для парсинга количеству (3:5:9)";
                    Console.WriteLine("Error {0}", ex);
                    throw new ArgumentOutOfRangeException(ex);
            }
        }
        /// <summary>
        /// Read as ExcelDataReader
        /// </summary>
        /// <returns></returns>
        IEnumerable<ShortCustomerInfo_excel> ReadFromXls()
        {
            var clientList = new List<ShortCustomerInfo_excel>();
            using (var stream = File.Open(originalFileName, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                var conf = new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false
                    }
                };
                var dataSet = reader.AsDataSet(conf).Tables[0];
                if (dataSet == null)
                    return clientList;
                //нумерация колонок от 0 (A = 0)
                int NameCol;
                int? TabNumberCol = null;
                int? PositionCol = null;
                int CardCol;
                //установка зависимостей используемых столбцов
                SelDataCollumns(dataSet.Columns.Count, 0,
                    out NameCol, out TabNumberCol, out PositionCol, out CardCol);
                foreach (DataRow row in dataSet.Rows)
                {
                    var customer = ReadRow(row, NameCol, TabNumberCol, PositionCol, CardCol);
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
        IEnumerable<ShortCustomerInfo_excel> ReadFromXlsx()
        {
            var clientList = new List<ShortCustomerInfo_excel>();
            using (var workbook = new XLWorkbook(originalFileName))
            {
                var dataSet = workbook.Worksheet(1);
                if (dataSet == null)
                    return clientList;
                //нумерация колонок от 1 (A = 1)
                int NameCol;
                int? TabNumberCol = null;
                int? PositionCol = null;
                int CardCol;
                //установка зависимостей используемых столбцов
                SelDataCollumns(dataSet.ColumnsUsed().Count(), 1,
                    out NameCol, out TabNumberCol, out PositionCol, out CardCol);
                foreach (var row in dataSet.RowsUsed())
                {
                    var customer = ReadRow(row, NameCol, TabNumberCol, PositionCol, CardCol);
                    if (customer == null || clientList.Contains(customer))
                        continue;
                    clientList.Add(customer);
                }
            }
            return clientList.ToArray();
        }
        public IEnumerable<ShortCustomerInfo_excel> GetClients()
        {
            try
            {
                log.Information("Read file {}", originalFileName);
                //if (originalFileName.Contains(".xlsx"))
                //return ReadFromXlsx().ToArray();
                return ReadFromXls().ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public static void CreateWorkbook<T>(String filePath, IList<T> list, string? title = null)
        {
            var dataset = ToDataSet<T>(list);
            CreateWorkbook(filePath, dataset, title);
        }
        public static void CreateWorkbook(String filePath, DataSet dataset, string? title = null)
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
